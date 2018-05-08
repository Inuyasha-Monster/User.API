using MediatR;

namespace Project.Domain.Events
{
    public class ProjectCreatedEvent : INotification
    {
        public AggregatesModel.Project Project { get; set; }
    }
}