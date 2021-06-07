using Identity.API.Options;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;

namespace Identity.API.Models.ViewModels
{
    public class ConsentInputModel
    {
        public string Button { get; set; }
        public IEnumerable<string> ScopesConsented { get; set; }
        public bool RememberConsent { get; set; }
        public string ReturnUrl { get; set; }
        public string Description { get; set; }

    }
}
