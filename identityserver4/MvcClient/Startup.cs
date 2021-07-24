using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MvcClient
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
            var authorityHost = _config.GetValue<string>("AuthorityHost");
            var apiOneHost = _config.GetValue<string>("ApiOneHost");

            services.AddAuthentication(config =>
            {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", config =>
                {
                    config.Authority = authorityHost;
                    config.ClientId = "client_id_mvc";
                    config.ClientSecret = "client_secret_mvc";
                    config.SaveTokens = true;

                    // if (_env.IsDevelopment())
                    // {
                    //     config.RequireHttpsMetadata = false;
                    // }
                    // response_type 
                    config.ResponseType = "code";

                    config.SignedOutCallbackPath = "/Home/Index";

                    // configure cookie claim mapping (rc.grandma map to RawCoding.Grandma at claim)
                    config.ClaimActions.MapUniqueJsonKey("RawCoding.Grandma", "rc.grandma");

                    // delete unnecessary claims
                    config.ClaimActions.DeleteClaim("amr");
                    config.ClaimActions.DeleteClaim("s_hash");

                    // two trips to load claims in to the cookie but id_token make smaller
                    config.GetClaimsFromUserInfoEndpoint = true;

                    // add scope if need
                    config.Scope.Clear();
                    // config.Scope.Add("openid");
                    // add additional scope
                    config.Scope.Add("rc.scope");
                    config.Scope.Add("openid");
                    config.Scope.Add("Ebiz.sap");
                    config.Scope.Add("ApiTwo.sec");
                    // in order to get refresh_token
                    config.Scope.Add("offline_access");
                });

            services.AddHttpClient("identityServer", config =>
            {
                config.BaseAddress = new Uri(authorityHost);
            });

            services.AddHttpClient("apiOne", config =>
            { 
                config.BaseAddress = new Uri(apiOneHost);
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // IdentityModelEventSource.ShowPII = true;
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
