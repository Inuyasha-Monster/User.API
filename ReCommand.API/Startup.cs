using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReCommand.API.EFData;

namespace ReCommand.API
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

            services.AddDbContext<ReCommandDbContext>(builder =>
            {

                builder.UseMySQL(Configuration.GetConnectionString("UserMysqlLocal"), x =>
                {
                    x.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                });

                //builder.UseMySQL(Configuration.GetConnectionString("UserMysql"));
            });

            services.AddCap(options =>
            {
                options.UseEntityFramework<ReCommandDbContext>();
                //options.UseRabbitMQ(Configuration.GetConnectionString("RabbitMq"));
                options.UseRabbitMQ("localhost");

                // 注册 Dashboard
                options.UseDashboard();

                // 注册节点到 Consul
                options.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 58809;
                    d.NodeId = 4;
                    d.NodeName = "CAP No.4 Node ReCommand.API";
                });
            });

            services.AddMvc().AddJsonOptions(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCap();

            app.UseMvc();
        }
    }
}
