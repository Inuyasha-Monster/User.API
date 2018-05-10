using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using ReCommand.API.EFData;
using ReCommand.API.EFData.Entities;
using ReCommand.API.IntergationEvents;
using ReCommand.API.Service;

namespace ReCommand.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : BaseController
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        private readonly ReCommandDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IContactService _contactService;


        public ValuesController(ReCommandDbContext dbContext, IUserService userService, IContactService contactService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _contactService = contactService;
        }

        [CapSubscribe("projectapi.projectcreated")]
        [NonAction]
        public async Task Process(ProjectCreatedIntergrationEvent @event)
        {
            var info = await _userService.GetBaseUserInfoAsync(@event.UserId);
            var contacts = await _contactService.GetContactListByUserIdAsync(@event.UserId);
            if (info != null && contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    var projectRecommand = new ProjectReCommand()
                    {
                        FromUserId = @event.UserId,
                        Company = @event.Company,
                        ProjectId = @event.ProjectId,
                        Introduction = @event.Introduction,
                        FromUserAvator = info.Avatar,
                        EnumReCommandType = EnumReCommandType.Friend,
                        FromUserName = info.Name,
                        CreateTime = @event.CreationDate,
                        ReCommandTime = DateTime.Now,
                        UserId = contact.UserId
                    };
                    await _dbContext.ProjectReCommands.AddAsync(projectRecommand);
                }
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
