using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Consul;
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
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Resilience.Http;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

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

            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;

                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));


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

            services.AddCap(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("UserMysqlLocal"));
                //options.UseRabbitMQ(Configuration.GetConnectionString("RabbitMq"));
                options.UseRabbitMQ("localhost");
                options.UseDashboard();
                options.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 58564;
                    d.NodeId = 2;
                    d.NodeName = "CAP No.2 Node Contact.API";
                });
            });

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
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env, IApplicationLifetime lifetime, IOptions<ServiceDisvoveryOptions> options, IConsulClient consulClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(LogLevel.Trace);

            lifetime.ApplicationStarted.Register(() =>
            {
                RegisterService(app, options, consulClient);
            });

            lifetime.ApplicationStopped.Register(() =>
            {
                DeRegisterService(app, options, consulClient);
            });

            app.UseCap();

            app.UseAuthentication();

            app.UseMvc();
        }


        private void DeRegisterService(IApplicationBuilder app,
            IOptions<ServiceDisvoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            }
        }

        private void RegisterService(IApplicationBuilder app,
            IOptions<ServiceDisvoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = new Uri(address, "HealthCheck").OriginalString
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = address.Port
                };

                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            }
        }
    }
}
