using System.Threading.Tasks;
using DotNetCore.CAP;
using ReCommand.API.IntergationEvents;

namespace ReCommand.API.IntergationEventHandlers
{
    public class ProjectCreatedIntergrationEventHandler : ICapSubscribe
    {
        [CapSubscribe("projectapi.projectcreated")]
        public async Task Process(ProjectCreatedIntergrationEvent @event)
        {

        }
    }
}