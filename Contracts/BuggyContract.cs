using BuggyBuddyAPI.Models;
using MongoDB.Bson;

namespace BuggyBuddyAPI.Contracts
{
    public interface IBuggyContract
    {
        public Task<List<BuggyModel>> GetAllBuggys();
        public Task<List<BuggyModel>> GetUserBuggyUrls(string userId);
        public Task<string> AddBuggyUrlToUser(string userId, string url);
        public Task<bool> DeleteBuggy(string buggyId, string userId);
    }
}
