using System.Collections.Generic;
using System.Threading.Tasks;
using Contact.API.Data;

namespace Contact.API.Repository
{
    public interface IContactRepository
    {
        Task<IEnumerable<FriendRequest>> GetFriendRequestListAsync(int userId);
        Task AddFriendAsync(FriendRequest request);
        Task PassFriendRequestAsync(int userId, int appliedUserId);
        Task RejectFriendRequestAsync(int userId, int appliedUserId);
    }
}