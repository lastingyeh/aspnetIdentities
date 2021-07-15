using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiOne
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
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config =>
                {
                    var authorityHost = _config.GetValue<string>("AuthorityHost");

                    config.Authority = authorityHost;
                    config.Audience = "Ebiz";
                    // if (_env.IsDevelopment())
                    // {
                    //     config.RequireHttpsMetadata = false;
                    // }
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("CRM", policyBuilder => policyBuilder.RequireClaim("Scope", "Ebiz.crm"));
                config.AddPolicy("SAP", policyBuilder => policyBuilder.RequireClaim("Scope", "Ebiz.sap"));
            });

            services.AddCors(config =>
            {
                var JsClientHost = _config.GetValue<string>("JsClientHost");
                var AngularClientHost = _config.GetValue<string>("AngularClientHost");

                config.AddPolicy("AllowHosts", policy =>
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(JsClientHost, AngularClientHost)
                );
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // IdentityModelEventSource.ShowPII = true;
            }

            app.UseCors("AllowHosts");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
