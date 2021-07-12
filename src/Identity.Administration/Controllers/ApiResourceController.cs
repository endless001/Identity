using Microsoft.AspNetCore.Mvc;
using IdentityServer4.EntityFramework.DbContexts;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.EntityFramework.Mappers;

namespace Identity.Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiResourceController : ControllerBase
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        public ApiResourceController(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> FindApiResourceById(string id)
        {
            var result = await _configurationDbContext.ApiResources.FindAsync(id);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveApiResource(string id)
        {
            var entity = await _configurationDbContext.ApiResources.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            var result = _configurationDbContext.ApiResources.Remove(entity);
            await _configurationDbContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> AddApiResource([FromBody] ApiResource model)
        {
            var result = _configurationDbContext.ApiResources.Add(model.ToEntity());
            await _configurationDbContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApiResource([FromBody] ApiResource model)
        {
            var result = _configurationDbContext.ApiResources.Update(model.ToEntity());
            await _configurationDbContext.SaveChangesAsync();
            return Ok(result);
        }
    }

}
