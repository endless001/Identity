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
    public class PersistedGrantController : ControllerBase
    {
        private readonly IPersistedGrantStore _persistedGrantStore;

        public PersistedGrantController(IPersistedGrantStore persistedGrantStore)
        {
            _persistedGrantStore = persistedGrantStore;
        }

    }
}
