using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReCommand.API.Service
{
    public interface IContactService
    {
        Task<List<Dtos.Contact>> GetContactListByUserIdAsync(int userId);
    }
}