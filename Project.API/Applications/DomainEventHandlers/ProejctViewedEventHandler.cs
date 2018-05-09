using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Project.API.Applications.IntegrationEvents;
using Project.Domain.Events;

namespace Project.API.Applications.DomainEventHandlers
{
    public class ProejctViewedEventHandler : INotificationHandler<ProejctViewedEvent>
    {
        private readonly ICapPublisher _capPublisher;

        public ProejctViewedEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Handle(ProejctViewedEvent notification, CancellationToken cancellationToken)
        {
            var @event=new ProejctViewedIntergrationEvent()
            {
                ProjectId = notification.ProjectViewer.ProjectId,
                UserId = notification.ProjectViewer.UserId,
                UserName = notification.ProjectViewer.UserName
            };
            await _capPublisher.PublishAsync("projectapi.projectviewed",@event);
        }
    }
}