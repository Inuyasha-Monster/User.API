using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApiGatewayWithConsul
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((webhost, builder) =>
                {
                    builder.SetBasePath(webhost.HostingEnvironment.ContentRootPath).AddJsonFile("Ocelot.json");
                })
                .UseUrls("http://0.0.0.0:50002")
                .UseStartup<Startup>()
                .Build();
    }
}
