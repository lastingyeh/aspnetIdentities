using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Data
{
    public static class ContextData
    {
        public static async Task MigrateAsync(IServiceScope scope)
        {
            await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

            await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

            await scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();
        }
        public static async Task SeedAsync(IServiceScope scope, IConfigurationSection config)
        {
            await AddUser(scope);
            await AddConfiguration(scope, config);
        }

        private static async Task AddUser(IServiceScope scope)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (!userManager.Users.Any())
            {
                var user = new IdentityUser("bob");

                await userManager.CreateAsync(user, "password");
                // place at id_token
                await userManager.AddClaimAsync(user, new Claim("rc.grandma", "bigcookie"));
                // place at access_token
                await userManager.AddClaimAsync(user, new Claim("rc.api.grandma", "big.api.cookie"));
            }
        }

        private static async Task AddConfiguration(IServiceScope scope, IConfigurationSection config)
        {
            var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            if (!context.Clients.Any())
            {
                foreach (var client in Configuration.GetClients(config))
                {
                    context.Clients.Add(client.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Configuration.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var apiResource in Configuration.GetApis())
                {
                    context.ApiResources.Add(apiResource.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var apiScope in Configuration.GetApiScopes())
                {
                    context.ApiScopes.Add(apiScope.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }
    }
}