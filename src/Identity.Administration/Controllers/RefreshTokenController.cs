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
    public class RefreshTokenController : ControllerBase
    {
        private readonly IRefreshTokenStore _refreshTokenStore;

        public RefreshTokenController(IRefreshTokenStore refreshTokenStore)
        {
            _refreshTokenStore = refreshTokenStore;
        }
    }
}
