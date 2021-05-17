using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Administration.Controllers
{
    public class UserConsentController : ControllerBase
    {
        private readonly IUserConsentStore _userConsentStore;
        public UserConsentController(IUserConsentStore userConsentStore)
        {
            _userConsentStore = userConsentStore;
        }

    }
}
