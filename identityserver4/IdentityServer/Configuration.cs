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
        // IdentityResources
        // IdentityResourceClaims
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
        // ApiResources
        // ApiResourceClaims
        // ApiResourceScopes
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("Ebiz")
                {
                    Scopes = {"Ebiz.sap", "Ebiz.crm"}
                },
                new ApiResource("ApiTwo", new string[]{"rc.api.grandma"})
                {
                    Scopes = {"ApiTwo.sec"}
                },
            };

        // Mapping to AddOpenIdConnect, eg: MvcClient
        // Clients
        // ClientSecrets
        // ClientRedirectUris
        // ClientGrantTypes
        // ClientCorsOrigins
        // ClientScopes
        // ClientPostLogoutRedirectUris
        public static IEnumerable<Client> GetClients(IConfigurationSection configSection) =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client_id",
                    ClientSecrets = {new Secret("client_secret".ToSha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"Ebiz.sap", "Ebiz.crm"}
                },
                new Client
                {
                    ClientId = "client_id_mvc",
                    ClientSecrets = {new Secret("client_secret_mvc".ToSha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {configSection["MvcClient:RedirectUri"]},
                    PostLogoutRedirectUris = {configSection["MvcClient:RedirectLogoutUri"]},
                    AllowedScopes =
                    {
                        "Ebiz.sap",
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
                // new Client
                // {
                //     ClientId = "client_id_js",
                //     AllowedGrantTypes = GrantTypes.Implicit,
                //     RedirectUris = {configSection["JsClient:RedirectUri"]},
                //     PostLogoutRedirectUris = {configSection["JsClient:RedirectLogoutUri"]},
                //     AllowedCorsOrigins = {"https://localhost:5004"},
                //     AllowedScopes =
                //     {
                //         "ApiOne.user",
                //         IdentityServerConstants.StandardScopes.OpenId,
                //         "rc.scope",
                //         "ApiTwo.sec",
                //     },
                //     AccessTokenLifetime = 1,
                //     AllowAccessTokensViaBrowser = true,
                // },
                new Client
                {
                    ClientId = "client_id_js",
                    AllowedGrantTypes = GrantTypes.Code,
                    // RequiredPkce
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = {configSection["JsClient:RedirectUri"]},
                    PostLogoutRedirectUris = {configSection["JsClient:RedirectLogoutUri"]},
                    AllowedCorsOrigins = {configSection["JsClient:Host"]},
                    AllowedScopes =
                    {
                        "Ebiz.sap",
                        IdentityServerConstants.StandardScopes.OpenId,
                        "rc.scope",
                        "ApiTwo.sec",
                    },
                    AccessTokenLifetime = 1,
                    AllowAccessTokensViaBrowser = true,
                },
                new Client
                {
                    ClientId = "angular",
                    AllowedGrantTypes = GrantTypes.Code,
                    // RequiredPkce
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = {configSection["AngularClient:RedirectUri"]},
                    PostLogoutRedirectUris = {configSection["AngularClient:RedirectLogoutUri"]},
                    AllowedCorsOrigins = {configSection["AngularClient:Host"]},
                    AllowedScopes =
                    {
                        "Ebiz.sap",
                        IdentityServerConstants.StandardScopes.OpenId,
                        "rc.scope",
                    },
                    AllowAccessTokensViaBrowser = true,
                }
            };

        // Set all scope depende on Client.AllowScopes
        // ApiScopes
        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope("Ebiz.sap"),
                new ApiScope("Ebiz.crm"),
                new ApiScope("ApiTwo.sec"),
                // new ApiScope("rc.api.grandma")
            };
    }
}
