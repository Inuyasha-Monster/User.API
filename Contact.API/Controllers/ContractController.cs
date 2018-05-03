using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Common;
using Contact.API.Data;
using Contact.API.Repository;
using Contact.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    public class ContractController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IContactRepository _contactRepository;

        public ContractController(IUserService userService, IContactRepository contactRepository)
        {
            _userService = userService;
            _contactRepository = contactRepository;
        }

        [HttpPost]
        [Route("apply-list")]
        public async Task<IActionResult> GetApplyList()
        {
            var friendRequests = await _contactRepository.GetFriendRequestListAsync(UserIdentity.CurrentUserId);
            return Json(friendRequests);
        }

        /// <summary>
        /// 添加好友申请
        /// </summary>
        /// <param name="userId">被申请人的ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-apply")]
        public async Task<IActionResult> AddApply(int userId)
        {
            var baseUserInfo = await _userService.GetBaseUserInfoAsync(userId);
            if (baseUserInfo == null) throw new UserContextException();
            await _contactRepository.AddFriendAsync(new FriendRequest()
            {
                AppliedUserId = UserIdentity.CurrentUserId,
                ApplyDateTime = DateTime.Now,
                ApplyStatus = Data.Enum.ApplyStatus.Waiting,
                Avatar = baseUserInfo.Avatar,
                Company = baseUserInfo.Company,
                Name = baseUserInfo.Name,
                Phone = baseUserInfo.Phone,
                Title = baseUserInfo.Title,
                UserId = userId
            });
            return Ok();
        }

        /// <summary>
        /// 通过好友申请
        /// </summary>
        /// <param name="userId">被申请的人的用户ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("pass-apply")]
        public async Task<IActionResult> Passed(int userId)
        {
            await _contactRepository.PassFriendRequestAsync(UserIdentity.CurrentUserId, userId);
            return Ok();
        }

        /// <summary>
        /// 拒绝好友申请
        /// </summary>
        /// <param name="userId">被申请的人的用户ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("reject-apply")]
        public async Task<IActionResult> Reject(int userId)
        {
            await _contactRepository.RejectFriendRequestAsync(UserIdentity.CurrentUserId, userId);
            return Ok();
        }
    }
}
