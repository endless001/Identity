using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationCodeController : ControllerBase
    {
        private readonly IAuthorizationCodeStore _authorizationCodeStore;

        public AuthorizationCodeController(IAuthorizationCodeStore authorizationCodeStore)
        {
            _authorizationCodeStore = authorizationCodeStore;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthorizationCode([FromQuery] string code)
        {
            var result = await _authorizationCodeStore.GetAuthorizationCodeAsync(code);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAuthorizationCode(string code)
        {
            await _authorizationCodeStore.RemoveAuthorizationCodeAsync(code);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> AddAuthorizationCode([FromBody] AuthorizationCode authorizationCode)
        {
            var result = await _authorizationCodeStore.StoreAuthorizationCodeAsync(authorizationCode);
            return Ok(result);
        }
    }
}
