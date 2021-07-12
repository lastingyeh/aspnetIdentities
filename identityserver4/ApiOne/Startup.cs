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
                    config.Audience = "ApiOne";

                    // if (_env.IsDevelopment())
                    // {
                    //     config.RequireHttpsMetadata = false;
                    // }
                });

            services.AddCors(config =>
            {
                var JsClientHost = _config.GetValue<string>("JsClientHost");

                config.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(JsClientHost)
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

            app.UseCors("AllowAll");

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
