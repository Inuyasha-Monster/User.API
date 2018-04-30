using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.API.Dtos;

namespace User.API.Controllers
{
    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity => new UserIdentity()
        {
            UserId = 1,
            Name = "djlnet"
        };
    }
}
