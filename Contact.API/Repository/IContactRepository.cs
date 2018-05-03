using System.Threading.Tasks;
using Contact.API.Dtos;

namespace Contact.API.Repository
{
    public interface IContactRepository
    {
        /// <summary>
        /// 更新好友基本信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        Task UpdateContactInfoAsync(int userId, BaseUserInfo info);
    }
}