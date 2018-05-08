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
    public class JoinProjectCommandHandler : IRequestHandler<JoinProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;

        public JoinProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.ProjectContributor.ProjectId);
            if (project == null) throw new ProjectDomainException($"project not find with projectId:{request.ProjectContributor.ProjectId}");
            project.ProjectContributors.Add(request.ProjectContributor);
            await _projectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
