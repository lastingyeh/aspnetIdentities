using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();
            var user = new IdentityUser("bob");

            userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
            // place at id_token
            userManager.AddClaimAsync(user, new Claim("rc.grandma", "bigcookie"))
                .GetAwaiter().GetResult();
            // place at access_token
            userManager.AddClaimAsync(user, new Claim("rc.api.grandma", "big.api.cookie"))
                .GetAwaiter().GetResult();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
