using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.API.Applications.Commands;
using Project.API.Applications.Services;
using Project.Domain.AggregatesModel;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ICommandService _commandService;


        public ProjectController(IMediator mediator, ICommandService commandService)
        {
            _mediator = mediator;
            _commandService = commandService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddProject([FromBody]Domain.AggregatesModel.Project project)
        {
            var cmd = new CreateProjectCommand() { Project = project };
            await _mediator.Send(cmd);
            return Ok();
        }

        [HttpPut]
        [Route("view/{projectId}")]
        public async Task<IActionResult> ViewProject(int projectId)
        {
            if (!await _commandService.IsRecommandProject(projectId, UserIdentity.CurrentUserId))
            {
                return BadRequest("不具有查看当前项目的权限");
            }
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
            if (!await _commandService.IsRecommandProject(contributor.ProjectId, UserIdentity.CurrentUserId))
            {
                return BadRequest("不具有查看当前项目的权限");
            }
            var cmd = new JoinProjectCommand() { ProjectContributor = contributor };
            await _mediator.Send(cmd);
            return Ok();
        }
    }
}
