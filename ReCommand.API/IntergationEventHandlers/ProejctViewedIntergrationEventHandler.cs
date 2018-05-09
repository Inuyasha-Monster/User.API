using System.Threading.Tasks;
using DotNetCore.CAP;
using ReCommand.API.IntergationEvents;

namespace ReCommand.API.IntergationEventHandlers
{
    public class ProejctViewedIntergrationEventHandler : ICapSubscribe
    {
        [CapSubscribe("projectapi.projectviewed")]
        public async Task Process(ProejctViewedIntergrationEvent @event)
        {
            
        }
    }
}