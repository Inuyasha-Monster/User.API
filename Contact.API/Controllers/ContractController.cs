using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Common;
using Contact.API.Data;
using Contact.API.Repository;
using Contact.API.Service;
using Contact.API.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    public class ContractController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IContactFriendRequestRepository _contactFriendRequestRepository;
        private readonly IContactRepository _contactRepository;

        public ContractController(IUserService userService, IContactFriendRequestRepository contactFriendRequestRepository, IContactRepository contactRepository)
        {
            _userService = userService;
            _contactFriendRequestRepository = contactFriendRequestRepository;
            _contactRepository = contactRepository;
        }

        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-list")]
        public async Task<IActionResult> GetApplyList()
        {
            var friendRequests = await _contactFriendRequestRepository.GetFriendRequestListAsync(UserIdentity.CurrentUserId);
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
            await _contactFriendRequestRepository.AddFriendAsync(new FriendRequest()
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
        [HttpPut]
        [Route("pass-apply")]
        public async Task<IActionResult> Passed(int userId)
        {
            await _contactFriendRequestRepository.PassFriendRequestAsync(UserIdentity.CurrentUserId, userId);

            var friendInfo = await _userService.GetBaseUserInfoAsync(userId);
            await _contactRepository.AddContactFriendAsync(UserIdentity.CurrentUserId, new Data.Contact()
            {
                Avatar = friendInfo.Avatar,
                Company = friendInfo.Company,
                Name = friendInfo.Name,
                Phone = friendInfo.Phone,
                Tags = friendInfo.Tags,
                Title = friendInfo.Title,
                UserId = userId
            });

            var baseUserInfo = await _userService.GetBaseUserInfoAsync(UserIdentity.CurrentUserId);
            await _contactRepository.AddContactFriendAsync(userId, new Data.Contact()
            {
                Avatar = baseUserInfo.Avatar,
                Company = baseUserInfo.Company,
                Name = baseUserInfo.Name,
                Phone = baseUserInfo.Phone,
                Tags = baseUserInfo.Tags,
                Title = baseUserInfo.Title,
                UserId = UserIdentity.CurrentUserId
            });

            return Ok();
        }

        /// <summary>
        /// 拒绝好友申请
        /// </summary>
        /// <param name="userId">被申请的人的用户ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("reject-apply")]
        public async Task<IActionResult> Reject(int userId)
        {
            await _contactFriendRequestRepository.RejectFriendRequestAsync(UserIdentity.CurrentUserId, userId);
            return Ok();
        }

        /// <summary>
        /// 删除好友(双向)
        /// </summary>
        /// <param name="friendUserId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("delete-friend")]
        public async Task<IActionResult> DeleteFriend(int friendUserId)
        {
            await _contactRepository.DeleteFriendAsync(UserIdentity.CurrentUserId, friendUserId);
            await _contactRepository.DeleteFriendAsync(friendUserId, UserIdentity.CurrentUserId);
            return Ok();
        }

        /// <summary>
        ///  获取朋友列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("friends")]
        public async Task<IActionResult> GetFriends()
        {
            var list = await _contactRepository.GetAllFriendListAsync(UserIdentity.CurrentUserId);
            return Json(list);
        }

        /// <summary>
        /// 给好友打标签
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("tags")]
        public async Task<IActionResult> Tags([FromBody]TagRequest request)
        {
            await _contactRepository.ContactTagsAsync(UserIdentity.CurrentUserId, request.FriendUserId, request.Tags);
            return Ok();
        }
    }
}
