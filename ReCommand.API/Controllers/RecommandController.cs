using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReCommand.API.EFData;

namespace ReCommand.API.Controllers
{
    [Route("api/[controller]")]
    public class RecommandController : BaseController
    {
        private readonly ReCommandDbContext _dbContext;

        public RecommandController(ReCommandDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("projects")]
        public async Task<IActionResult> Get()
        {
            var projectReCommands = await _dbContext.ProjectReCommands.Include(x=>x.ProjectReferenceUsers)
                .Where(x=>x.UserId == UserIdentity.CurrentUserId).ToListAsync();
            return Ok(projectReCommands);
        }
    }
}