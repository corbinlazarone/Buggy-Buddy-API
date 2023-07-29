using MongoDB.Bson;

namespace BuggyBuddyAPI.Models.Request
{
    public class BuggyRequest
    {
        public string? UserId { get; set; }
        public string? URL { get; set; }
    }
}
