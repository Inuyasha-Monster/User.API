using MediatR;

namespace Project.API.Applications.Commands
{
    public class CreateProjectCommand : IRequest
    {
        public Domain.AggregatesModel.Project Project { get; set; }
    }
}