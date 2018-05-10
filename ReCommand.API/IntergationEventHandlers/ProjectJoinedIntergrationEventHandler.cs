using System.Threading.Tasks;
using DotNetCore.CAP;
using ReCommand.API.EFData;
using ReCommand.API.IntergationEvents;

namespace ReCommand.API.IntergationEventHandlers
{
    public class ProjectJoinedIntergrationEventHandler //: ICapSubscribe
    {
        private readonly ReCommandDbContext _dbContext;

        public ProjectJoinedIntergrationEventHandler(ReCommandDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [CapSubscribe("projectapi.projectjoined")]
        public async Task Process(ProjectJoinedIntergrationEvent @event)
        {
            await Task.CompletedTask;
        }
    }
}