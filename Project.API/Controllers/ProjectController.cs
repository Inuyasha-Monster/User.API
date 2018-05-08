using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.API.Applications.Commands;
using Project.Domain.AggregatesModel;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : BaseController
    {
        private readonly IMediator _mediator;


        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddProject([FromBody]Domain.AggregatesModel.Project project)
        {
            var cmd = new CreateProjectCommand() { Project = project };
            var projectResult = await _mediator.Send(cmd);
            return Ok(projectResult);
        }

        [HttpPut]
        [Route("view/{projectId}")]
        public async Task<IActionResult> ViewProject(int projectId)
        {
            var cmd = new ViewProjectCommand()
            {
                ProjectViewer = new ProjectViewer()
                {
                    Avator = UserIdentity.Avatar,
                    ProjectId = projectId,
                    UserName = UserIdentity.Name,
                    UserId = UserIdentity.CurrentUserId,
                    CreateTime = DateTime.Now
                }
            };
            await _mediator.Send(cmd);
            return Ok();
        }

        [HttpPut]
        [Route("join")]
        public async Task<IActionResult> JoinProject([FromBody] ProjectContributor contributor)
        {
            var cmd = new JoinProjectCommand() { ProjectContributor = contributor };
            await _mediator.Send(cmd);
            return Ok();
        }
    }
}
