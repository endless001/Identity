using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rbac.Attributes;
using Rbac.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAccess]
    public class ApiController : ControllerBase
    {
        private readonly IResourceService _resourceService;

        public ApiController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }
        [HttpGet()]
   
        public IActionResult Get()
        {
            return Content("1");
        }

        [HttpPost("{id?}")]
        public async Task<IActionResult> Post(int id)
        {
            var resources = await _resourceService.GetRoleResourcesAsync(id);

            return Ok(resources);
        }
    }
}
