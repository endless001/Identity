using Identity.Administration.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rbac.Attributes;
using Rbac.EntityFramework.Storage.DbContexts;
using Rbac.EntityFramework.Storage.Mappers;
using Rbac.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using RoleEntity = Rbac.EntityFramework.Storage.Entities.Role;

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


        [HttpGet]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Role>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Role>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
          
            var totalItems = await _configurationDbContext.Roles
                .LongCountAsync();

            var itemsOnPage = await _configurationDbContext.Roles
                .OrderBy(c => c.Id)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = MapToResponse(itemsOnPage, totalItems, pageIndex, pageSize);

            return Ok(model);
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


        private PaginatedItemsViewModel<Role> MapToResponse(List<RoleEntity> items, long count, int pageIndex, int pageSize)
        {
            var itemsOnPage = new List<Role>();
            items.ForEach(i =>
            {
                itemsOnPage.Add(i.ToModel());
            });

           return  new PaginatedItemsViewModel<Role>(pageIndex, pageSize, count, itemsOnPage);
        }
    }
}
