using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Common;
using Contact.API.Data;
using Contact.API.Dtos;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Contact.API.Repository
{
    public class MongoContactRepository : IContactRepository
    {
        private readonly MongoContactDbContext _dbContext;
        private readonly ILogger<MongoContactRepository> _logger;

        public MongoContactRepository(MongoContactDbContext dbContext, ILogger<MongoContactRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task UpdateContactInfoAsync(int userId, BaseUserInfo info)
        {
            var book = await _dbContext.ContactCollection.FindAsync(x => x.UserId == userId);
            if (book == null) throw new UserContextException();
            FilterDefinition<ContactBook> filterDefinition = new ExpressionFilterDefinition<ContactBook>(x => x.Contacts.Select(c => c.UserId).Contains(info.UserId));

            UpdateDefinition<ContactBook> updateDefinition = new BsonDocumentUpdateDefinition<ContactBook>(new BsonDocument(new Dictionary<string, string>()
            {
                {"ContactBook.$.Name",info.Name },
                {"ContactBook.$.Avatar",info.Avatar },
                {"ContactBook.$.Company",info.Company },
                {"ContactBook.$.Title",info.Title }
            }));

            var updateResult = await _dbContext.ContactCollection.UpdateManyAsync(filterDefinition, updateDefinition);
            _logger.LogInformation($"{nameof(updateResult.MatchedCount)} {updateResult.MatchedCount},{nameof(updateResult.ModifiedCount)} {updateResult.ModifiedCount}");
        }

        public async Task AddContactFriend(int userId, Data.Contact contact)
        {
            var book = await _dbContext.ContactCollection.FindAsync(x => x.UserId == userId);
            if (book == null)
            {
                await _dbContext.ContactCollection.InsertOneAsync(new ContactBook()
                {
                    UserId = userId
                });
            }

            var filter = Builders<ContactBook>.Filter.Eq(x => x.UserId, userId);
            var update = Builders<ContactBook>.Update.AddToSet(x => x.Contacts, contact);

            await _dbContext.ContactCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteFriend(int userId, int friendUserId)
        {
            var book = await _dbContext.ContactCollection.FindAsync(x => x.UserId == userId);
            if (book == null) throw new UserContextException();
            var filter = Builders<ContactBook>.Filter.Eq(x => x.UserId, userId);

            var removeFilter = Builders<ContactBook>.Filter.Eq("UserId", friendUserId);
            var update = Builders<ContactBook>.Update.PullFilter("Contacts.$.UserId", removeFilter);

            await _dbContext.ContactCollection.FindOneAndUpdateAsync(filter, update);
        }
    }
}