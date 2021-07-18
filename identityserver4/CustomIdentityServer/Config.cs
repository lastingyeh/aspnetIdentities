using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace CustomIdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("dataEventRecordsApi", "Data Event Records API",
                    new List<string>{"role", "admin", "user", "dataEventRecords", "dataEventRecords.admin", "dataEventRecords.user" })
                {
                    ApiSecrets = {new Secret("dataEventRecordsSecret".Sha256())},
                    Scopes = {"dataEventRecordsScope"}
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope
                {
                    Name = "dataEventRecordsScope",
                    DisplayName = "Scope for the dataEventRecords ApiResource",
                    UserClaims = {"role", "admin", "user", "dataEventRecords", "dataEventRecords.admin", "dataEventRecords.user"}
                }
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client{
                    ClientId = "resourceownerclient",
                    ClientSecrets = new List<Secret> { new Secret("dataEventRecordsSecret".Sha256())},

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 3600,
                    IdentityTokenLifetime = 3600,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    SlidingRefreshTokenLifetime = 30,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AlwaysSendClientClaims = true,
                    Enabled = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "dataEventRecordsScope",
                    }
                }
            };
    }
}
