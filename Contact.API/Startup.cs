using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Contact.API.Common;
using Contact.API.Data;
using Contact.API.Infrastructure;
using Contact.API.Options;
using Contact.API.Repository;
using Contact.API.Service;
using DnsClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Resilience.Http;

namespace Contact.API
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

            services.AddOptions();
            services.Configure<AppSetting>(x =>
            {
                x.MongoDbConnectionString = Configuration["MongoDbConnectionString"].ToString();
                x.MongoDbDatabase = Configuration["MongoDbDatabase"].ToString();
            });

            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));

            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;

                return new LookupClient(IPAddress.Parse(serviceConfiguration.Consul.DnsEndpoint.Address), serviceConfiguration.Consul.DnsEndpoint.Port);

            });

            services.AddSingleton<ResilienceClientFactory>(sp =>
            {
                var loggger = sp.GetRequiredService<ILogger<ResilienceClientFactory>>();
                var logggerHttpClinet = sp.GetRequiredService<ILogger<ResilientHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var retryCount = 5;
                var expCountAllowedBeforeBreak = 5;
                var factory = new ResilienceClientFactory(loggger, httpContextAccessor, retryCount, expCountAllowedBeforeBreak, logggerHttpClinet);
                return factory;
            });

            services.AddSingleton<IHttpClient>(sp =>
            {
                var resilienceClientFactory = sp.GetRequiredService<ResilienceClientFactory>();
                return resilienceClientFactory.GetResilientHttpClient();
            });

            services.AddSingleton<MongoContactDbContext>();

            services.AddScoped<IContactFriendRequestRepository, MongoContactFriendRequestRepository>();

            services.AddScoped<IContactRepository, MongoContactRepository>();

            services.AddScoped<IUserService, UserService>();

            services.AddLogging(x => x.AddConsole());

            services.AddMvc();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:4000";
                    options.Audience = "gateway_contactapi";
                    options.RequireHttpsMetadata = false;
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(LogLevel.Trace);

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
