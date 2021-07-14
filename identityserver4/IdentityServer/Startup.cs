using System.Security.Cryptography.X509Certificates;
using System.IO;
using IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseSqlServer(connectionString);
                // config.UseInMemoryDatabase("Memory");
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

            var assembly = typeof(Startup).Assembly.GetName().Name;

            // cert setting
            // var filePath = Path.Combine(_env.ContentRootPath, "ids_cert.pfx");
            // var certificate = new X509Certificate2(filePath, _config.GetValue<string>("CertPassword"));

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
                // .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                // .AddInMemoryApiResources(Configuration.GetApis())
                // .AddInMemoryClients(Configuration.GetClients(_config.GetSection("ClientSettings")))
                // .AddInMemoryApiScopes(Configuration.GetApiScopes())
                .AddDeveloperSigningCredential();
            // .AddSigningCredential(certificate);

            services.AddAuthentication()
                .AddFacebook(config =>
                {
                    config.AppId = _config.GetValue<string>("Facebook:AppId");
                    config.AppSecret = _config.GetValue<string>("Facebook:AppSecret");
                });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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