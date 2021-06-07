using Identity.API.Infrastructure.Services;
using Identity.API.Models;
using Identity.API.Models.ViewModels;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMessageService _messageService;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IConfiguration _configuration;
        private readonly IEventService _events;
        private readonly IClientStore _clientStore;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService,
            IMessageService messageService,
            IIdentityServerInteractionService interaction,
            IConfiguration configuration,
            IEventService events,
            IClientStore clientStore,
            ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _messageService = messageService;
            _interaction = interaction;
            _configuration = configuration;
            _events = events;
            _clientStore = clientStore;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {

          var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
          if (context?.IdP != null)
          {
            throw new NotImplementedException("External login is not implemented!");
          }

          var vm = await BuildLoginViewModelAsync(returnUrl, context);
          ViewData["ReturnUrl"] = returnUrl;
          return View();
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = await _accountService.PasswordSignInAsync(model.AccountName, model.Password);
                if (account != null)
                {
                    var tokenLifetime = _configuration.GetValue("TokenLifetimeMinutes", 120);
                    var props = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifetime),
                        AllowRefresh = true,
                        RedirectUri = model.ReturnUrl
                    };

                    if (model.RememberMe)
                    {
                        var permanentTokenLifetime = _configuration.GetValue("PermanentTokenLifetimeDays", 365);
                        props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(permanentTokenLifetime);
                        props.IsPersistent = true;
                    }

                    await _events.RaiseAsync(new UserLoginSuccessEvent(account.AccountName, account.AccountId.ToString(),
                      account.AccountName));

                    var claims = new List<Claim>
                    {
                      new Claim("sub",account.AccountId.ToString()),
                      new Claim("accountName",account.AccountName??string.Empty),
                      new Claim("avatar",account.Avatar??string.Empty),
                      new Claim("phone",account.Phone??string.Empty),
                      new Claim("email",account.Email??string.Empty)
                    };

                    var identity = new ClaimsIdentity(claims, "Passport");
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(principal,props);

                    return Redirect(_interaction.IsValidReturnUrl(model.ReturnUrl) ? model.ReturnUrl : "~/");
                }

                ModelState.AddModelError(string.Empty, "Invalid username or password.");

            }

            var vm = await BuildLoginViewModelAsync(model);

            ViewData["ReturnUrl"] = model.ReturnUrl;

            return View(vm);
        }

        public async Task<IActionResult> SendVerifyCode(string email)
        {
            Random rd = new Random();
            var verifyCode = rd.Next(100000, 1000000);
            var content = $"Your verify code is:{verifyCode}";

            await _messageService.SendMail(email, content);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                // if the user is not authenticated, then just show logged out page
                return await Logout(new LoginViewModel { LogoutId = logoutId });
            }

            //Test for Xamarin.
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                //it's safe to automatically sign-out
                return await Logout(new LoginViewModel { LogoutId = logoutId });
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            var vm = new LoginViewModel
            {
                LogoutId = logoutId
            };
            return Redirect("login");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LoginViewModel model)
        {
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                if (model.LogoutId == null)
                {
                    // if there's no current logout context, we need to create one
                    // this captures necessary info from the current logged in user
                    // before we signout and redirect away to the external IdP for signout
                    model.LogoutId = await _interaction.CreateLogoutContextAsync();
                }

                string url = "/Account/Logout?logoutId=" + model.LogoutId;

                try
                {

                    // hack: try/catch to handle social providers that throw
                    await HttpContext.SignOutAsync(idp, new AuthenticationProperties
                    {
                        RedirectUri = url
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "LOGOUT ERROR: {ExceptionMessage}", ex.Message);
                }
            }

            await HttpContext.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

            return Redirect(logout?.PostLogoutRedirectUri);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterEmailAsync(model.Email, model.Password);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Registration Failed.");
                    return View();
                }
            }

            if (returnUrl != null)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                    return Redirect(returnUrl);
                else
                    if (ModelState.IsValid)
                    return RedirectToAction("login", "account", new { returnUrl = returnUrl });
                else
                    return View(model);
            }

            return View();
        }



        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {

          var allowLocal = true;
          if (context?.Client.ClientId != null)
          {
            var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client != null)
            {
              allowLocal = client.EnableLocalLogin;
            }
          }

          return new LoginViewModel
            {
                ReturnUrl = returnUrl,
                AccountName = context?.LoginHint,
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl, context);
            vm.AccountName = model.AccountName;
            vm.RememberMe = model.RememberMe;
            return vm;
        }
    }
}
