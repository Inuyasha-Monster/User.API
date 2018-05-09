using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Project.API.Applications.IntegrationEvents;
using Project.Domain.Events;

namespace Project.API.Applications.DomainEventHandlers
{
    public class ProjectJoinedEventHandler : INotificationHandler<ProjectJoinedEvent>
    {
        private readonly ICapPublisher _capPublisher;

        public ProjectJoinedEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Handle(ProjectJoinedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectJoinedIntergrationEvent()
            {
                 ProjectId = notification.ProjectContributor.ProjectId,
                 UserId = notification.ProjectContributor.UserId,
                 UserName = notification.ProjectContributor.UserName
            };
            await _capPublisher.PublishAsync("projectapi.projectjoined",@event);
        }
    }
}