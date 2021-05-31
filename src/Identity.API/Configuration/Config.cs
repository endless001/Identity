using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Identity.API.Configuration
{
  public class Config
  {
    public static IEnumerable<ApiResource> GetApis()
    {
      return new List<ApiResource>
      {
        new ApiResource("account", "Account Service"),
        new ApiResource("contact", "Contact Service"),
        new ApiResource("download", "Download Service"),
        new ApiResource("upload", "Upload Service"),
      };
    }

    public static IEnumerable<IdentityResource> GetResources()
    {
      return new List<IdentityResource>
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),

      };
    }

    public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
    {
      return new List<Client>
      {
        new Client()
        {

          ClientId = "api",
          ClientName = "Api Client",
          ClientSecrets = new List<Secret>
          {
            new Secret("secret".Sha256())
          },
          AllowedGrantTypes = {GrantType.ResourceOwnerPassword, "sms"},
          AllowedScopes = new List<string>
          {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            IdentityServerConstants.StandardScopes.OfflineAccess,
            "account",
            "contact",
            "download",
            "upload",
          },
          AllowOfflineAccess = true
        },
        new Client
        {
          ClientId = "chat",
          ClientName = "Chat Client",
          ClientSecrets = new List<Secret>
          {
            new Secret("secret".Sha256())
          },
          ClientUri = $"{clientsUrl["Chat"]}",
          AllowedGrantTypes = GrantTypes.Implicit,
          AllowAccessTokensViaBrowser = true,
          RequireConsent = true,
          AllowOfflineAccess = true,
          AlwaysIncludeUserClaimsInIdToken = true,
          RedirectUris = new List<string>
          {
            $"{clientsUrl["Chat"]}/signin-oidc"
          },
          PostLogoutRedirectUris = new List<string>
          {
            $"{clientsUrl["Chat"]}/signout-callback-oidc"
          },
          AllowedScopes = new List<string>
          {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            IdentityServerConstants.StandardScopes.OfflineAccess,
            "account",
            "contact",
            "download",
            "upload",
          },
          AllowedCorsOrigins = new List<string> {"http://localhost:3000"},
          AccessTokenLifetime = 60 * 60 * 2, // 2 hours
          IdentityTokenLifetime = 60 * 60 * 2 // 2 hours
        }
      };
    }
  }
}
