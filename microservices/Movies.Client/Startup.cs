using System;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Movies.Client.ApiServices;
using Movies.Client.HttpHandlers;

namespace Movies.Client
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddHttpContextAccessor();

            services.AddTransient<AuthenticationDelegatingHandler>();

            services.AddScoped<IMovieApiService, MovieApiService>();

            // ApiGateway Client
            services.AddHttpClient("MovieAPIClient", client =>
            {
                client.BaseAddress = new Uri(Configuration["ApiGatewayUrl"]); // API GATEWAY URL
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            }).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            // Identityserver Client
            services.AddHttpClient("IDPClient", client =>
            {
                client.BaseAddress = new Uri(Configuration["AuthorityUrl"]);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });

            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opts =>
            {
                opts.Authority = Configuration["AuthorityUrl"];

                opts.ClientId = "movies_mvc_client";
                opts.ClientSecret = "secret";
                opts.ResponseType = "code id_token";

                opts.RequireHttpsMetadata = false;

                opts.Scope.Add("address");
                opts.Scope.Add("email");
                opts.Scope.Add("roles");
                opts.Scope.Add("movieAPI");

                opts.ClaimActions.DeleteClaim("sid");
                opts.ClaimActions.DeleteClaim("idp");
                opts.ClaimActions.DeleteClaim("s_hash");
                opts.ClaimActions.DeleteClaim("auth_time");

                opts.ClaimActions.MapUniqueJsonKey("role", "role");

                opts.SaveTokens = true;
                opts.GetClaimsFromUserInfoEndpoint = true;

                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.GivenName,
                    RoleClaimType = JwtClaimTypes.Role,
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
