using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Project.API.Applications.IntegrationEvents;
using Project.Domain.Events;

namespace Project.API.Applications.DomainEventHandlers
{
    public class ProjectCreatedEventHandler : INotificationHandler<ProjectCreatedEvent>
    {
        private readonly ICapPublisher _capPublisher;

        public ProjectCreatedEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectCreatedIntergrationEvent()
            {
                Company = notification.Project.Company,
                ProjectId = notification.Project.Id,
                UserId = notification.Project.UserId,
                Introduction = notification.Project.Introduction
            };
            await _capPublisher.PublishAsync("projectapi.projectcreated",@event);
        }
    }
}