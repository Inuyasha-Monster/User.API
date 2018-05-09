using System.Threading.Tasks;
using DotNetCore.CAP;
using ReCommand.API.EFData;
using ReCommand.API.IntergationEvents;

namespace ReCommand.API.IntergationEventHandlers
{
    public class ProjectCreatedIntergrationEventHandler : ICapSubscribe
    {
        private readonly ReCommandDbContext _dbContext;

        public ProjectCreatedIntergrationEventHandler(ReCommandDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [CapSubscribe("projectapi.projectcreated")]
        public async Task Process(ProjectCreatedIntergrationEvent @event)
        {
            await Task.CompletedTask;
        }
    }
}