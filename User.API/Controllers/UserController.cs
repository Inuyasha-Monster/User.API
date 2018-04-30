using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using User.API.Data;
using User.API.Models;

namespace User.API.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly UserDbContext _dbContext;
        private readonly ILogger<UserController> _logger;

        public UserController(UserDbContext dbContext, ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var user = await _dbContext.AppUsers
                .AsNoTracking()
                .Include(x => x.Properties)
                .SingleOrDefaultAsync(x => x.Id == UserIdentity.UserId);
            if (user == null)
            {
                throw new UserOperationException($"错误的用户上下文Id {UserIdentity.UserId}");
            }
            return Json(user);
        }

        [HttpPatch]
        [Route("")]
        public async Task<IActionResult> Patch([FromBody]JsonPatchDocument<AppUser> patch)
        {
            var user = await _dbContext.AppUsers
                .SingleOrDefaultAsync(x => x.Id == UserIdentity.UserId);
            if (user == null)
            {
                throw new UserOperationException($"错误的用户上下文Id {UserIdentity.UserId}");
            }
            patch.ApplyTo(user);

            foreach (var item in user?.Properties)
            {
                _dbContext.Entry(item).State = EntityState.Detached;
            }

            var currentPros = user.Properties;
            var originPros = await _dbContext.AppUserProperties.AsNoTracking().Where(x => x.AppUserId == user.Id).ToListAsync();
            var allPros = originPros.Union(currentPros).Distinct();

            var removeRang = originPros.Except(currentPros);
            _dbContext.RemoveRange(removeRang);

            var addRang = allPros.Except(originPros);
            await _dbContext.AddRangeAsync(addRang);

            await _dbContext.SaveChangesAsync();
            return Json(user);
        }

        [HttpPost]
        [Route("check-or-create")]
        public async Task<IActionResult> CheckOrCreate(string phone)
        {
            var user = await _dbContext.AppUsers.SingleOrDefaultAsync(x => x.Phone == phone);

            if (user != null) return Ok(user.Id);
            user = new AppUser()
            {
                Phone = phone
            };
            await _dbContext.AppUsers.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return Ok(user.Id);
        }
    }
}
