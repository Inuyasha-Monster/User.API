using System.Collections.Generic;
using System.Threading.Tasks;
using Contact.API.Data;

namespace Contact.API.Repository
{
    public class MongoContactRepository : IContactRepository
    {
        private readonly MongoContactDbContext _dbContext;

        public MongoContactRepository(MongoContactDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FriendRequest>> GetFriendRequestListAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddFriendAsync(FriendRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task PassFriendRequestAsync(int userId, int appliedUserId)
        {
            throw new System.NotImplementedException();
        }

        public async Task RejectFriendRequestAsync(int userId, int appliedUserId)
        {
            throw new System.NotImplementedException();
        }
    }
}