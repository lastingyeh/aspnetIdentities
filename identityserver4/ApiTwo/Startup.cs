using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiTwo
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
                    config.Authority = _config["AuthorityHost"];
                    config.Audience = "ApiTwo";

                    if (_env.IsDevelopment())
                    {
                        config.RequireHttpsMetadata = false;
                    }
                });

            services.AddHttpClient("identityServer", config =>
            {
                config.BaseAddress = new Uri(_config["AuthorityHost"]);
            });
            // .ConfigurePrimaryHttpMessageHandler(_ =>
            // {
            //     var handler = new HttpClientHandler();
            //     if (_env.IsDevelopment())
            //     {
            //         handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            //     }
            //     return handler;
            // });

            services.AddHttpClient("apiOne", config =>
            {
                config.BaseAddress = new Uri(_config["ApiOneHost"]);
            });
            // .ConfigurePrimaryHttpMessageHandler(_ =>
            // {
            //     var handler = new HttpClientHandler();
            //     if (_env.IsDevelopment())
            //     {
            //         handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            //     }
            //     return handler;
            // });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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