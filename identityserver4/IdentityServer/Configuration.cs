using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
    public static class Configuration
    {
        // configure in id_token
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                // new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name="rc.scope",
                    UserClaims = {"rc.grandma"}
                }
            };
        // Set resources with Api with scope and claims
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("ApiOne")
                {
                    Scopes = {"ApiOne.user"}
                },
                new ApiResource("ApiTwo", new string[]{"rc.api.grandma"})
                {
                    Scopes = {"ApiTwo.sec"}
                },
            };

        // Mapping to AddOpenIdConnect, eg: MvcClient
        public static IEnumerable<Client> GetClients(IConfigurationSection configSection) =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client_id",
                    ClientSecrets = {new Secret("client_secret".ToSha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"ApiOne.user"}
                },
                new Client
                {
                    ClientId = "client_id_mvc",
                    ClientSecrets = {new Secret("client_secret_mvc".ToSha256())},

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = {configSection["MvcClient:RedirectUri"]},

                    AllowedScopes =
                    {
                        "ApiOne.user",
                        "ApiTwo.sec",
                        IdentityServerConstants.StandardScopes.OpenId,
                        // IdentityServerConstants.StandardScopes.Profile,
                        "rc.scope"
                    },
                    // add scope [offline_access]
                    AllowOfflineAccess = true,
                    // RequireConsent = false
                    // put all the claims in the id_token
                    // AlwaysIncludeUserClaimsInIdToken = true,
                },
                new Client
                {
                    ClientId = "client_id_js",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris = {configSection["JsClient:RedirectUri"]},
                    AllowedCorsOrigins = {"https://localhost:5004"},

                    AllowedScopes =
                    {
                        "ApiOne.user",
                        IdentityServerConstants.StandardScopes.OpenId,
                        "rc.scope",
                        "ApiTwo.sec",
                    },

                    AccessTokenLifetime = 1,

                    AllowAccessTokensViaBrowser = true,
                }
            };

        // Set all scope depende on Client.AllowScopes
        public static IEnumerable<ApiScope> GetScopes() =>
            new List<ApiScope>
            {
                new ApiScope("ApiOne.user"),
                new ApiScope("ApiTwo.sec"),
                // new ApiScope("rc.api.grandma")
            };
    }
}
