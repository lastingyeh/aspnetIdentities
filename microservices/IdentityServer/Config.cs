using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients(IConfiguration configuration) =>
            new Client[]
            {
                // API
                new Client
                {
                    ClientId = "movieClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedScopes = {"movieAPI"}
                },
                // MVC Client
                new Client
                {
                    ClientId = "movies_mvc_client",
                    ClientName = "Movies MVC Web App",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = false,
                    AllowRememberConsent = false,
                    RedirectUris = new List<string>
                    {
                        $"{configuration["MovieClientUrl"]}/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{configuration["MovieClientUrl"]}/signout-callback-oidc"
                    },
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        "movieAPI",
                        "roles",
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[] { new ApiScope("movieAPI", "Movie API") };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("MoiveAPIs"){Scopes = {"movieAPI"}}
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "Your role(s)", new List<string>{"role"})
            };

        // public static List<TestUser> TestUsers =>
        //     new List<TestUser>
        //     {
        //         new TestUser
        //         {
        //             SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
        //             Username = "mehmet",
        //             Password = "swn",
        //             Claims = new List<Claim>
        //             {
        //                 new Claim(JwtClaimTypes.GivenName, "mehmet"),
        //                 new Claim(JwtClaimTypes.FamilyName, "ozkaya")
        //             }
        //         }
        //     };
    }
}
