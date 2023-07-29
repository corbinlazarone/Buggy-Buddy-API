using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BuggyBuddyAPI.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("firstName")]
        public string? FirstName { get; set; }

        [BsonElement("LastName")]
        public string? LastName { get; set;}

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

    }
}
