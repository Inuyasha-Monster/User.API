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
                var user = new UserIdentity();

                user.Avatar = User.Claims.First(x => x.Type == "avatar").Value;
                user.Company = User.Claims.First(x => x.Type == "company").Value;
                user.Name = User.Claims.First(x => x.Type == "name").Value;
                user.Phone = User.Claims.First(x => x.Type == "phone").Value;
                user.Title = User.Claims.First(x => x.Type == "title").Value;
                user.CurrentUserId = int.Parse(User.Claims.First(x => x.Type == "sub").Value);

                return user;
            }
        }
    }
}
