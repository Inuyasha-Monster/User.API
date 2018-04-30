using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace API.Geteway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((x, y) =>
                {
                    y.SetBasePath(x.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("Ocelot.json");
                })
                .UseUrls("http://localhost:4000")
                .UseStartup<Startup>()
                .Build();
    }
}
