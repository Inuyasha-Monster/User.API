using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using User.API.Data;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly UserDbContext _dbContext;

        public ValuesController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _dbContext.AppUsers.SingleOrDefaultAsync(x => x.Name == "djlnet");
            if (user == null)
                return NotFound();
            var json = $"docker test ci {JsonConvert.SerializeObject(user)}";
            return Content(json);
        }
    }
}
