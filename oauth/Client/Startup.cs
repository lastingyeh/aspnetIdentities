using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Client.CustomAuthorizaton;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client
{
    public class Startup
    {
        public readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(config =>
            {
                // check the cookie to confirm that authenticated
                config.DefaultAuthenticateScheme = "ClientCookie";
                // when signIn that need to deal out a cookie
                config.DefaultSignInScheme = "ClientCookie";
                // use to check if allowed to do something
                config.DefaultChallengeScheme = "OurServer";
            })
                .AddCookie("ClientCookie")
                .AddOAuth("OurServer", config =>
                {
                    var authServerHost = _config["AuthServerHost"];

                    config.ClientId = "client_id";
                    config.ClientSecret = "client_secret";
                    config.CallbackPath = "/oauth/callback";
                    config.AuthorizationEndpoint = $"{authServerHost}/oauth/authorize";
                    // config.AuthorizationEndpoint = Configuration["AuthServerHostUrl"];
                    config.TokenEndpoint = $"{authServerHost}/oauth/token";
                    // set to retrieve access_token 
                    config.SaveTokens = true;

                    config.Events = new OAuthEvents
                    {
                        OnCreatingTicket = context =>
                        {
                            // when access_token retrieved
                            var accessToken = context.AccessToken;
                            var base64Payload = accessToken.Split('.')[1];
                            var bytes = Base64UrlTextEncoder.Decode(base64Payload);
                            var payload = Encoding.UTF8.GetString(bytes);
                            var claims = JsonSerializer.Deserialize<IDictionary<string, object>>(payload);
                            // add to context.User.Claims
                            foreach (var claim in claims)
                            {
                                context.Identity.AddClaim(new Claim(claim.Key, claim.Value.ToString()));
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("Claim.DoB", policyBuilder =>
                {
                    policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddHttpClient().AddHttpContextAccessor();
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
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}