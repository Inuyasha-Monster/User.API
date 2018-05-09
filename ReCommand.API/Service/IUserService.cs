using System.Threading.Tasks;
using ReCommand.API.Dtos;

namespace ReCommand.API.Service
{
    public interface IUserService
    {
        Task<BaseUserInfo> GetBaseUserInfoAsync(int userId);
    }
}