using Identity.Administration.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rbac.EntityFramework.Storage.DbContexts;
using Rbac.EntityFramework.Storage.Mappers;
using Rbac.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using UserEntity = Rbac.EntityFramework.Storage.Entities.User;

namespace Identity.Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ConfigurationDbContext _configurationDbContext;

        public UserController(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<User>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Role>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {

            var totalItems = await _configurationDbContext.Users
                .LongCountAsync();

            var itemsOnPage = await _configurationDbContext.Users
                .OrderBy(c => c.Id)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = MapToResponse(itemsOnPage, totalItems, pageIndex, pageSize);

            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _configurationDbContext.Roles.FindAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Role model)
        {
            _configurationDbContext.Roles.Add(model.ToEntity());
            var result = await _configurationDbContext.SaveChangesAsync();
            return Ok(result);
        }


        private PaginatedItemsViewModel<User> MapToResponse(List<UserEntity> items, long count, int pageIndex, int pageSize)
        {
            var itemsOnPage = new List<User>();
            items.ForEach(i =>
            {
                itemsOnPage.Add(i.ToModel());
            });

            return new PaginatedItemsViewModel<User>(pageIndex, pageSize, count, itemsOnPage);
        }

    }
}
