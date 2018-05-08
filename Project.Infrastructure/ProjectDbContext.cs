using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Domain.SeedWork;

namespace Project.Infrastructure
{
    public class ProjectDbContext : DbContext , IUnitOfWork
    {
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}
