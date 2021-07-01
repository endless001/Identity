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
    public class DeviceFlowController : ControllerBase
    {
        private readonly IDeviceFlowStore _deviceFlowStore;

        public DeviceFlowController(IDeviceFlowStore deviceFlowStore)
        {
            _deviceFlowStore = deviceFlowStore;
        }
        [HttpGet]
        public async Task<IActionResult> FindByDeviceCode(string deviceCode)
        {
            var result = await _deviceFlowStore.FindByDeviceCodeAsync(deviceCode);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeteleDevice(string deviceCode)
        {
            await _deviceFlowStore.RemoveByDeviceCodeAsync(deviceCode);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> AddDevice([FromQuery]string deviceCode, string userCode, [FromBody]DeviceCode data)
        {
            await _deviceFlowStore.StoreDeviceAuthorizationAsync(deviceCode, userCode, data);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDevice([FromQuery]string userCode, [FromBody] DeviceCode device)
        {
            await _deviceFlowStore.UpdateByUserCodeAsync(userCode, device);
            return Ok();
        }


    }
}
