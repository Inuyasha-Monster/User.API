using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace API.Geteway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            var authenticationProviderKey = "finbook";

            services.AddAuthentication()
                .AddIdentityServerAuthentication(authenticationProviderKey, x =>
                {
                    x.Authority = "http://localhost:6000"; // 授权地址
                    x.ApiName = "gateway_api";
                    x.SupportedTokens = SupportedTokens.Both;
                    x.ApiSecret = "secret";
                    x.RequireHttpsMetadata = false;
                });

            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseOcelot().Wait();
        }
    }
}
