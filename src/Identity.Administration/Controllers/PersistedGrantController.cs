using IdentityServer4.EntityFramework.DbContexts;
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
        private readonly PersistedGrantDbContext _persistedGrantDbContext;

        public PersistedGrantController(PersistedGrantDbContext persistedGrantDbContext)
        {
            _persistedGrantDbContext = persistedGrantDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> FindPersistedGrantById(string id)
        {
            var result = await _persistedGrantDbContext.PersistedGrants.FindAsync(id);
            return Ok(result);
        }
    }
}
