using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// todo: 从token中获取当前请求用户的userid
        /// </summary>
        protected UserIdentity UserIdentity => new UserIdentity() { CurrentUserId = 1 };
    }
}
