using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Domain.Events;

namespace Project.API.Applications.DomainEventHandlers
{
    public class ProjectJoinedEventHandler : INotificationHandler<ProjectJoinedEvent>
    {
        public async Task Handle(ProjectJoinedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}