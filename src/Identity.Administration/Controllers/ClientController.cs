using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Mappers;
namespace Identity.Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController:ControllerBase
    {
        private readonly IClientStore _clientStore;
        private readonly ConfigurationDbContext _configurationDbContext;
        public ClientController(IClientStore clientStore, ConfigurationDbContext configurationDbContext)
        {
            _clientStore = clientStore;
            _configurationDbContext = configurationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> FindClientById(string clientId)
        {
            var result = await _clientStore.FindClientByIdAsync(clientId);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveClient(string clientId)
        {
            var entity = await _configurationDbContext.Clients.FindAsync(clientId);
            if (entity == null)
            {
                return NotFound();
            }

            var result = _configurationDbContext.Clients.Remove(entity);
            await _configurationDbContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> AddClient([FromBody] Client model)
        {
            var result = _configurationDbContext.Clients.Add(model.ToEntity());
            await _configurationDbContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClient([FromBody] Client model)
        {
            var result = _configurationDbContext.Clients.Update(model.ToEntity());
            await _configurationDbContext.SaveChangesAsync();
            return Ok(result);
        }


    }
}
