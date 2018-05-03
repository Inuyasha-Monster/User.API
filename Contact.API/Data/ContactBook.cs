using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Contact.API.Data
{
    /// <summary>
    /// 通讯录
    /// </summary>
    public class ContactBook
    {
        [BsonId]
        public ObjectId ObjectId { get; set; }
        public int UserId { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}