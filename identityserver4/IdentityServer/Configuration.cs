using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("ApiOne"){Scopes = {"ApiOne.secret"}}
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client_id",
                    ClientSecrets = {new Secret("client_secret".ToSha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"ApiOne.secret"}
                }
            };

        public static IEnumerable<ApiScope> GetScopes() =>
            new List<ApiScope>
            {
                new ApiScope("ApiOne.secret")
            };
    }
}