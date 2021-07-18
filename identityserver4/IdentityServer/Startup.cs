using IdentityServer.Data;
using IdentityServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityServer4.Services;

namespace IdentityServer
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;
        public Startup(IConfiguration config, IHostEnvironment env)
        {
            _env = env;
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            var inMemory = _config.GetValue<bool>("InMemory");

            services.AddDbContext<AppDbContext>(config =>
            {
                if (inMemory)
                {
                    config.UseInMemoryDatabase("Memory");
                }
                else
                {
                    config.UseSqlServer(connectionString);
                }
            });

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // authorize config settings
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            if (inMemory)
            {
                services.AddIdentityServer(opts =>
                    {
                        opts.Events.RaiseSuccessEvents = true;
                        opts.Events.RaiseFailureEvents = true;
                        opts.Events.RaiseErrorEvents = true;
                    })
                    .AddAspNetIdentity<IdentityUser>()
                    .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                    .AddInMemoryApiResources(Configuration.GetApis())
                    .AddInMemoryClients(Configuration.GetClients(_config.GetSection("ClientSettings")))
                    .AddInMemoryApiScopes(Configuration.GetApiScopes())
                    .AddDeveloperSigningCredential()
                    .Services.AddTransient<IProfileService, ProfileService>();
            }
            else
            {
                // cert setting
                // var filePath = Path.Combine(_env.ContentRootPath, "ids_cert.pfx");
                // var certificate = new X509Certificate2(filePath, _config.GetValue<string>("CertPassword"));

                var assembly = typeof(Startup).Assembly.GetName().Name;

                services.AddIdentityServer()
                    .AddAspNetIdentity<IdentityUser>()
                    .AddConfigurationStore(opts =>
                    {
                        opts.ConfigureDbContext = dbBuilder => dbBuilder.UseSqlServer(connectionString,
                            sqlOptions => sqlOptions.MigrationsAssembly(assembly));
                    })
                    .AddOperationalStore(opts =>
                    {
                        opts.ConfigureDbContext = dbBuilder => dbBuilder.UseSqlServer(connectionString,
                            sqlOptions => sqlOptions.MigrationsAssembly(assembly));
                    })
                    .AddDeveloperSigningCredential();
                // .AddSigningCredential(certificate);
            }

            services.AddAuthentication()
                .AddFacebook(config =>
                {
                    config.AppId = _config.GetValue<string>("Facebook:AppId");
                    config.AppSecret = _config.GetValue<string>("Facebook:AppSecret");
                });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
