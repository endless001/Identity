using IdentityServer4.EntityFramework.Stores;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly ResourceStore _resourceStore;
        public ResourceController(ResourceStore resourceStore)
        {
            _resourceStore = resourceStore;
        }
    }
}
