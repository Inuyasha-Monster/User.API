using System.Threading.Tasks;
using DotNetCore.CAP;
using ReCommand.API.EFData;
using ReCommand.API.IntergationEvents;

namespace ReCommand.API.IntergationEventHandlers
{
    public class ProejctViewedIntergrationEventHandler //: ICapSubscribe
    {
        private readonly ReCommandDbContext _dbContext;

        public ProejctViewedIntergrationEventHandler(ReCommandDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [CapSubscribe("projectapi.projectviewed")]
        public async Task Process(ProejctViewedIntergrationEvent @event)
        {
            await Task.CompletedTask;
        }
    }
}