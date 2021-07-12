using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.EntityFramework.DbContexts;

namespace Identity.Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityResourceController : ControllerBase
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        public IdentityResourceController(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> FindIdentityResourceById(int id)
        {
            var result = await _configurationDbContext.IdentityResources.FindAsync(id);
            return Ok(result);
        }
    }
}
