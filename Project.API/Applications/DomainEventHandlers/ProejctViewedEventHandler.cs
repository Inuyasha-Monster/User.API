using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Domain.Events;

namespace Project.API.Applications.DomainEventHandlers
{
    public class ProejctViewedEventHandler : INotificationHandler<ProejctViewedEvent>
    {
        public async Task Handle(ProejctViewedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}