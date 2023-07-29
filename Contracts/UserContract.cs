using BuggyBuddyAPI.Models;
using BuggyBuddyAPI.Models.Request;

namespace BuggyBuddyAPI.Contracts
{
    public interface IUserContract
    {
        public Task<List<UserModel>> GetAllUsers();
        public Task<string> CreateUser(UserRequests _userRequests);
        public Task<object> Login(string email, string password);
        public Task<UserModel> GetUserById(string userId);
        public Task<string> GetUserIDByEmail(string email);
        public Task<bool> DeleteUser(string userId);
    }
}
