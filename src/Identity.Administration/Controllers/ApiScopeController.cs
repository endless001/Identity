using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiScopeController : ControllerBase
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        public ApiScopeController(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> FindApiScopeById(string id)
        {
            var result = await _configurationDbContext.ApiScopes.FindAsync(id);
            return Ok(result);
        }
    }
}
