
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;

namespace Identity.API.Models.ViewModels
{
    public class ScopeViewModel
    {
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }

        public ScopeViewModel() { }
        public ScopeViewModel(IdentityResource identity, bool check)
        {
            Value = identity.Name;
            DisplayName = identity.DisplayName ?? identity.Name;
            Description = identity.Description;
            Emphasize = identity.Emphasize;
            Required = identity.Required;
            Checked = check || identity.Required;
        
        }

        public ScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
        {
            var displayName = apiScope.DisplayName ?? apiScope.Name;
            if (!string.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
            {
                displayName += ":" + parsedScopeValue.ParsedParameter;
            }

            Value = parsedScopeValue.RawValue;
            DisplayName = displayName;
            Description = apiScope.Description;
            Emphasize = apiScope.Emphasize;
            Required = apiScope.Required;
            Checked = check || apiScope.Required;
        }
    }
}
