using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CustomResourceServer.Data;
using CustomResourceServer.DataProtection;
using CustomResourceServer.Repositories;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CustomResourceServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostEnv;

        public Startup(IConfiguration config, IWebHostEnvironment hostEnv)
        {
            _config = config;
            _hostEnv = hostEnv;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = _config.GetConnectionString("DefaultConnection");
            // var cert = new X509Certificate2(Path.Combine(_hostEnv.ContentRootPath, "damienbodserver.pfx"), "");

            // services.AddDataProtection()
            //     .SetApplicationName("ResourceServer")
            //     .ProtectKeysWithCertificate(cert)
            //     .AddKeyManagementOptions(options =>
            //         options.XmlRepository = new SqlXmlRepository(
            //             new DataProtectionDbContext(
            //                 new DbContextOptionsBuilder<DataProtectionDbContext>().UseSqlite(connection).Options
            //             )
            //         )
            //     );

            services.AddDbContext<DataEventRecordContext>(opts => opts.UseSqlite(connection));

            services.AddCors(opts =>
            {
                opts.AddPolicy("corsGlobalPolicy", policy =>
                {
                    policy.WithHeaders("*").WithMethods("*").AllowCredentials();
                });
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = _config.GetValue<string>("AuthorityHost");
                    options.ApiName = "dataEventRecordsApi";
                    options.ApiSecret = _config.GetValue<string>("ApiSecret");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("dataEventRecordsAdmin", policyAdmin =>
                {
                    policyAdmin.RequireClaim("role", "dataEventRecords.admin");
                });
                options.AddPolicy("dataEventRecordsUser", policyUser =>
                {
                    policyUser.RequireClaim("role", "dataEventRecords.user");
                });
                options.AddPolicy("dataEventRecords", policyUser =>
                {
                    policyUser.RequireClaim("scope", "dataEventRecordsScope");
                });
            });

            services.AddScoped<IDataEventRecordRepository, DataEventRecordRepository>();
            
            services.AddControllers();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));
            }

            app.UseCors("corsGlobalPolicy");

            app.UseStaticFiles();

            app.UseHttpsRedirection();

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
