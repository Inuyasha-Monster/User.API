using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReCommand.API.Service
{
    public class ContactService : IContactService
    {


        public async Task<List<Dtos.Contact>> GetContactListByUserIdAsync(int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}