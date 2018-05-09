using System.Threading.Tasks;
using DotNetCore.CAP;
using ReCommand.API.IntergationEvents;

namespace ReCommand.API.IntergationEventHandlers
{
    public class ProjectJoinedIntergrationEventHandler : ICapSubscribe
    {
        [CapSubscribe("projectapi.projectjoined")]
        public async Task Process(ProjectJoinedIntergrationEvent @event)
        {

        }
    }
}