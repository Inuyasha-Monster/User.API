using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Project.API.Dtos;

namespace Project.API.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 从token中获取当前请求用户的userid以及基本信息
        /// </summary>
        protected UserIdentity UserIdentity
        {
            get
            {
                var user = new UserIdentity
                {
                    Avatar = User.Claims.First(x => x.Type == "avatar").Value,
                    Company = User.Claims.First(x => x.Type == "company").Value,
                    Name = User.Claims.First(x => x.Type == "name").Value,
                    Phone = User.Claims.First(x => x.Type == "phone").Value,
                    Title = User.Claims.First(x => x.Type == "title").Value,
                    CurrentUserId = int.Parse(User.Claims.First(x => x.Type == "sub").Value)
                };
                return user;
            }
        }
    }
}
