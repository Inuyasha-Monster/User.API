using System;
using Contact.API.Data.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Contact.API.Data
{
    public class FriendRequest
    {
        [BsonId]
        public ObjectId ObjectId { get; set; }

        /// <summary>
        /// 被加的好友用户ID
        /// </summary>
        public int UserId { get; set; }

        public string Name { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }

        /// <summary>
        /// 申请人用户ID
        /// </summary>
        public int AppliedUserId { get; set; }

        public ApplyStatus ApplyStatus { get; set; }

        public DateTime ApplyDateTime { get; set; }
    }
}