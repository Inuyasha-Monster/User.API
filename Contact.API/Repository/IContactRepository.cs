using System.Collections.Generic;
using System.Threading.Tasks;
using Contact.API.Dtos;

namespace Contact.API.Repository
{
    public interface IContactRepository
    {
        /// <summary>
        /// 更新好友基本信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task UpdateContactInfoAsync(BaseUserInfo info);

        Task AddContactFriendAsync(int userId, Data.Contact contact);

        Task DeleteFriendAsync(int userId, int friendUserId);

        Task<IEnumerable<Data.Contact>> GetAllFriendListAsync(int userId);

        Task ContactTagsAsync(int userId, int friendUserId, string[] tags);
    }
}