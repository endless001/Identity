using Microsoft.AspNetCore.Mvc;
using IdentityServer4.EntityFramework.DbContexts;
using System.Threading.Tasks;

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
    }
}
