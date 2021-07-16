using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rbac.Attributes;
using Rbac.EntityFramework.Storage.DbContexts;
using Rbac.EntityFramework.Storage.Mappers;
using Rbac.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAccess]
    public class RoleController : ControllerBase
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        

        public RoleController(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindRoleById(int id)
        {
            var result = await _configurationDbContext.Roles.FindAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] Role model)
        {
            _configurationDbContext.Roles.Add(model.ToEntity());
            var result = await _configurationDbContext.SaveChangesAsync();
            return Ok(result);
        }
    }
}
