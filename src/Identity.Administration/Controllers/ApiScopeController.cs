using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
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

        [HttpDelete]
        public async Task<IActionResult> RemoveApiScope(string id)
        {
            var entity = await _configurationDbContext.ApiScopes.FindAsync(id);
            if (entity == null)
            {
                return BadRequest();
            }

            var result = _configurationDbContext.ApiScopes.Remove(entity);
            await _configurationDbContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> AddApiScope([FromBody] ApiScope model)
        {
            var result = _configurationDbContext.ApiScopes.Add(model.ToEntity());
            await _configurationDbContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApiScope([FromBody] ApiScope model)
        {
            var result = _configurationDbContext.ApiScopes.Update(model.ToEntity());
            await _configurationDbContext.SaveChangesAsync();
            return Ok(result);
        }
    }
}
