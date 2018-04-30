using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using User.API.Models;

namespace User.API.Data
{
    public class UserContextSeed
    {
        public static async Task SeedAsync(IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory, int retry = 0)
        {
            var retryCount = retry;
            try
            {
                using (var scope = applicationBuilder.ApplicationServices.CreateScope())
                {
                    var context = (UserDbContext)scope.ServiceProvider.GetService(typeof(UserDbContext));
                    var logger = (ILogger<UserContextSeed>)scope.ServiceProvider.GetService(typeof(ILogger<UserContextSeed>));
                    logger.LogDebug("Begin userContextSeed SeedAsync");
                    context.Database.Migrate();

                    if (!context.AppUsers.Any())
                    {
                        context.AppUsers.Add(new AppUser()
                        {
                            Company = "fuck",
                            Name = "djlnet",
                            Title = "fuck"
                        });
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                retryCount++;
                var logger = loggerFactory.CreateLogger(typeof(UserContextSeed));
                logger.LogError(e.Message);
                await Task.Delay(TimeSpan.FromSeconds(retryCount));
                if (retryCount <= 5)
                {
                    await SeedAsync(applicationBuilder, loggerFactory, retryCount);
                }
            }
        }
    }
}
