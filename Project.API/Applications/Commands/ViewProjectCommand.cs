using MediatR;
using Project.Domain.AggregatesModel;

namespace Project.API.Applications.Commands
{
    public class ViewProjectCommand: IRequest
    {
        public ProjectViewer ProjectViewer { get; set; }
    }
}