using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Domain.AggregatesModel;
using Project.Domain.Exceptions;

namespace Project.API.Applications.Commands
{
    public class ViewProjectCommandHandler : IRequestHandler<ViewProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;

        public ViewProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task Handle(ViewProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.ProjectViewer.ProjectId);
            if (project == null) throw new ProjectDomainException($"project not find with ptojectId:{request.ProjectViewer.ProjectId}");
            if (project.UserId == request.ProjectViewer.UserId)
            {
                throw new ProjectDomainException("you can not view your own project");
            }
            project.AddViewer(request.ProjectViewer.UserName, request.ProjectViewer.Avator, request.ProjectViewer.UserId);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
