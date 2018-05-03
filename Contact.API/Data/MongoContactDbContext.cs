using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Common;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
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
            var list = _mongoDatabase.ListCollections().ToList().Select(x => x["name"].AsString);
            if (!list.Contains(name))
            {
                _mongoDatabase.CreateCollection(name);
            }
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
