using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Administration.Controllers
{
    public class ReferenceTokenController : ControllerBase
    {
        private readonly IReferenceTokenStore _referenceTokenStore;

        public ReferenceTokenController(IReferenceTokenStore referenceTokenStore)
        {
            _referenceTokenStore = referenceTokenStore;
        }

        public async Task<IActionResult> GetReferenceToken(string handle)
        {
            var result = await _referenceTokenStore.GetReferenceTokenAsync(handle);
            return Ok(result);
        }

        public async Task<IActionResult> AddReferenceToken(Token token)
        {
            var result = await _referenceTokenStore.StoreReferenceTokenAsync(token);
            return Ok(result);
        }


    }
}
