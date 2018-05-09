using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.API.Applications.Commands;
using Project.API.Applications.Queries;
using Project.API.Applications.Services;
using Project.Domain.AggregatesModel;
using Project.Domain.Exceptions;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ICommandService _commandService;
        private readonly IProjectQueries _projectQueries;


        public ProjectController(IMediator mediator, ICommandService commandService, IProjectQueries projectQueries)
        {
            _mediator = mediator;
            _commandService = commandService;
            _projectQueries = projectQueries;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetProjectListByUserId(int userId)
        {
            var result = await _projectQueries.GetProjectListByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpGet]
        [Route("my")]
        public async Task<IActionResult> GetMyProjectList()
        {
            var result = await _projectQueries.GetProjectListByUserIdAsync(UserIdentity.CurrentUserId);
            return Ok(result);
        }

        [HttpGet]
        [Route("detail/{projectId}")]
        public async Task<IActionResult> GetProjectDetail(int projectId)
        {
            var result = await _projectQueries.GetProjectDetailAsync(projectId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Route("recommand/{projectId}")]
        public async Task<IActionResult> RecommandProjectDetail(int projectId)
        {
            var b = await _commandService.IsRecommandProject(projectId, UserIdentity.CurrentUserId);
            if (!b) return BadRequest("无权查看此项目");
            var o = await _projectQueries.GetProjectDetailAsync(projectId);
            if (o == null) return NotFound();
            return Ok(o);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddProject([FromBody]Domain.AggregatesModel.Project project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            var cmd = new CreateProjectCommand() { Project = project };
            cmd.Project.UserId = UserIdentity.CurrentUserId;
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
            if (contributor == null) throw new ArgumentNullException(nameof(contributor));
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
