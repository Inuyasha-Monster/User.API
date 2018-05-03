using System.Threading.Tasks;
using Contact.API.Dtos;

namespace Contact.API.Service
{
    public interface IUserService
    {
        Task<BaseUserInfo> GetBaseUserInfoAsync(int userId);
    }
}