using System;
using Contact.API.Data.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Contact.API.Data
{
    /// <summary>
    /// 好友申请模型
    /// </summary>
    [BsonIgnoreExtraElements]
    public class FriendRequest
    {
        public FriendRequest()
        {
            ApplyStatus = ApplyStatus.Waiting;
            ApplyDateTime = DateTime.Now;
        }

        [BsonId]
        public ObjectId ObjectId { get; set; }

        /// <summary>
        /// 当前用户ID
        /// </summary>
        public int UserId { get; set; }

        public string Name { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }

        /// <summary>
        /// 好友用户ID
        /// </summary>
        public int AppliedUserId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public ApplyStatus ApplyStatus { get; set; }

        public DateTime ApplyDateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}