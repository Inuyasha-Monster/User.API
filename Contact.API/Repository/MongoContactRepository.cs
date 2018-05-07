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
            var book = await _dbContext.ContactCollection.FindSync(x => x.UserId == userId).FirstOrDefaultAsync();
            if (book == null)
            {
                book = new ContactBook() { UserId = userId };
                await _dbContext.ContactCollection.InsertOneAsync(book);
            }

            if (book.Contacts != null && book.Contacts.Any() &&
                book.Contacts.Select(x => x.UserId).Contains(info.UserId))
            {
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
        }

        public async Task AddContactFriendAsync(int userId, Data.Contact contact)
        {
            var book = await _dbContext.ContactCollection.FindSync(x => x.UserId == userId).FirstOrDefaultAsync();
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

        public async Task DeleteFriendAsync(int userId, int friendUserId)
        {
            var book = await _dbContext.ContactCollection.FindSync(x => x.UserId == userId).FirstOrDefaultAsync();
            if (book == null)
            {
                await _dbContext.ContactCollection.InsertOneAsync(new ContactBook()
                {
                    UserId = userId
                });
            }
            //var filter = Builders<ContactBook>.Filter.Eq(x => x.UserId, userId);

            //var removeFilter = Builders<ContactBook>.Filter.Eq("UserId", friendUserId);

            if (book != null && book.Contacts.Any() && book.Contacts.Select(x => x.UserId).ToList().Contains(friendUserId))
            {
                var update = Builders<ContactBook>.Update.PullFilter(x => x.Contacts, f => f.UserId == friendUserId);

                await _dbContext.ContactCollection.FindOneAndUpdateAsync(x => x.UserId == userId, update);
            }
        }

        public async Task<IEnumerable<Data.Contact>> GetAllFriendListAsync(int userId)
        {
            var book = await _dbContext.ContactCollection.FindSync(x => x.UserId == userId).FirstOrDefaultAsync();
            if (book == null)
            {
                await _dbContext.ContactCollection.InsertOneAsync(new ContactBook()
                {
                    UserId = userId
                });
                return new List<Data.Contact>();
            }
            var contactBook = book.Contacts;
            return contactBook;
        }

        public async Task ContactTagsAsync(int userId, int friendUserId, string[] tags)
        {
            var book = await _dbContext.ContactCollection.FindSync(x => x.UserId == userId).FirstOrDefaultAsync();
            if (book == null)
            {
                await _dbContext.ContactCollection.InsertOneAsync(new ContactBook()
                {
                    UserId = userId
                });
            }

            if (book != null && book.Contacts.Any() && book.Contacts.Select(x => x.UserId).ToList()
                    .Contains(friendUserId))
            {
                var filter = Builders<ContactBook>.Filter.And(Builders<ContactBook>.Filter.Eq(x => x.UserId, userId), Builders<ContactBook>.Filter.Eq("Contacts.UserId", friendUserId));
                var update = Builders<ContactBook>.Update.Set("Contacts.$.Tags", tags);
                await _dbContext.ContactCollection.UpdateOneAsync(filter, update);
            }

        }
    }
}