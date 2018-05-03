using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Common;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class MongoContactDbContext
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;

        public MongoContactDbContext(IOptionsSnapshot<AppSetting> optionsSnapshot)
        {
            if (_mongoClient == null)
            {
                _mongoClient = new MongoClient(optionsSnapshot.Value.MongoDbConnectionString);
            }
            if (_mongoDatabase == null)
            {
                _mongoDatabase = _mongoClient.GetDatabase(optionsSnapshot.Value.MongoDbDatabase);
            }
        }

        private void CheckOrCreateCollection(string name)
        {
            // todo: 检查是否存在，否则就创建
        }

        public IMongoCollection<FriendRequest> FriendRequestCollection
        {
            get
            {
                CheckOrCreateCollection("FriendRequestCollection");
                return _mongoDatabase.GetCollection<FriendRequest>("FriendRequestCollection");
            }
        }

        public IMongoCollection<ContactBook> ContactCollection
        {
            get
            {
                CheckOrCreateCollection("ContactCollection");
                return _mongoDatabase.GetCollection<ContactBook>("ContactCollection");
            }
        }
    }
}
