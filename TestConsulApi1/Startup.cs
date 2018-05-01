using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TestConsulApi1
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
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            lifetime.ApplicationStarted.Register(() =>
            {
                var consulClient = new ConsulClient(x =>
                {
                    x.Address = new Uri("http://localhost:8500");
                });

                var check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    HTTP = "http://localhost:5000/api/heathcheck",
                    Interval = TimeSpan.FromSeconds(10)
                };

                var registration = new AgentServiceRegistration()
                {
                    ID = "api1",
                    Address = "localhost",
                    Check = check,
                    Name = "api",
                    Port = 5000,
                    Tags = new string[] { "test_api" }
                };

                consulClient.Agent.ServiceRegister(registration).Wait();
            });

            lifetime.ApplicationStopped.Register(() =>
            {
                var consulClient = new ConsulClient(x =>
                {
                    x.Address = new Uri("http://localhost:8500");
                });

                consulClient.Agent.ServiceDeregister("api1").Wait();
            });

            app.UseMvc();
        }
    }
}
