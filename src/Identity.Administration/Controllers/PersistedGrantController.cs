using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
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
    public class PersistedGrantController : ControllerBase
    {
        private readonly PersistedGrantDbContext _persistedGrantDbContext;

        public PersistedGrantController(PersistedGrantDbContext persistedGrantDbContext)
        {
            _persistedGrantDbContext = persistedGrantDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _persistedGrantDbContext.PersistedGrants.FindAsync(id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _persistedGrantDbContext.PersistedGrants.FindAsync(id);
            if (entity == null)
            {
                return BadRequest();
            }

            var result = _persistedGrantDbContext.PersistedGrants.Remove(entity);
            await _persistedGrantDbContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Create([FromBody] PersistedGrant model)
        {
            var result = _persistedGrantDbContext.PersistedGrants.Add(model.ToEntity());
            await _persistedGrantDbContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] PersistedGrant model)
        {
            var result = _persistedGrantDbContext.PersistedGrants.Update(model.ToEntity());
            await _persistedGrantDbContext.SaveChangesAsync();
            return Ok(result);
        }
    }
}
