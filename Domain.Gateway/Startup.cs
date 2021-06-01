using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup IHttpClientFactory
            services.AddHttpClient();

            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));

            services.AddOcelot().AddConsul()
                .AddDelegatingHandler<MissingBodyDelegatingHandler>(true);
            services.AddSwaggerForOcelot(Configuration);

            var settings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            var Audiences = settings.ApiResources.Split(',').ToList();
            var authenticationBuilder = services.AddAuthentication();

            foreach (string Audience in Audiences)
            {
                authenticationBuilder.AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>(Audience, null);
            }

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Gateway", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<AppSettings> appSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
                opt.OAuthClientId(appSettings.Value.ClientId);
                opt.OAuthClientSecret(appSettings.Value.ClientSecret);
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            app.UseOcelot().Wait();
        }
    }
}
