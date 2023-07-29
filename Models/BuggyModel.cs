using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BuggyBuddyAPI.Models
{
    public class BuggyModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userID")]
        public string? UserId { get; set; }

        [BsonElement("buggyUrl")]
        public string? BuggyUrl { get; set; }
    }
}
