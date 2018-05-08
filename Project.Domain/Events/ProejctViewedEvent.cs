using MediatR;
using Project.Domain.AggregatesModel;

namespace Project.Domain.Events
{
    public class ProejctViewedEvent : INotification
    {
        public ProjectViewer ProjectViewer { get; set; }
    }
}