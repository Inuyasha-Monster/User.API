using System.Threading.Tasks;
using Contact.API.Dtos;

namespace Contact.API.Service
{
    public class UserService : IUserService
    {
        public async Task<BaseUserInfo> GetBaseUserInfoAsync(int userId)
        {
            throw new System.NotImplementedException();
        }  
    }
}