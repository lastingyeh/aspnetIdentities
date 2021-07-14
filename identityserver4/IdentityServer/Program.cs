using System;
using IdentityServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            try
            {
                using var scope = host.Services.CreateScope();

                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                ContextData.MigrateAsync(scope).GetAwaiter().GetResult();

                ContextData.SeedAsync(scope, config.GetSection("ClientSettings")).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
