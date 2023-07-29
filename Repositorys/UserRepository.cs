using BuggyBuddyAPI.Contracts;
using BuggyBuddyAPI.Models;
using BuggyBuddyAPI.Models.Request;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BuggyBuddyAPI.Repositorys
{
    public class UserRepository : IUserContract
    {
        private readonly IMongoCollection<UserModel> _userCollection;

        public UserRepository(
            IOptions<DatabaseSettings> _databaseSettings)
        {
            var mongoClient = new MongoClient(
                _databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                _databaseSettings.Value.DataBaseName);

            _userCollection = mongoDatabase.GetCollection<UserModel>(
                _databaseSettings.Value.UsersCollectionName);
        }

        // Test Connection
        public async Task<List<UserModel>> GetAllUsers()
        {
            var result = await _userCollection.Find(_ => true).ToListAsync();
            return result;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="password"></param>
        /// <returns>New User</returns>
        public async Task<string> CreateUser(UserRequests _userRequests)
        {
            var newUser = new UserModel
            {
                FirstName = _userRequests.FirstName,
                LastName = _userRequests.LastName,
                Email = _userRequests.Email,
                Password = HashPassword(_userRequests.PassWord),
            };

            await _userCollection.InsertOneAsync(newUser);

            if (newUser == null)
            {
                return "ERROR creating user!";
            }

            return "User Created Successfully!";
        }

        /// <summary>
        /// Grabs User if the user is exist and password matches password in db.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"><x/param>
        /// <returns>user</returns>
        public async Task<object> Login(string email, string password)
        {
            var user = await _userCollection.Find(u => u.Email == email).SingleOrDefaultAsync();

            if (user != null && VerifyPassword(password, user.Password))
            {
                return user;
            }

            // returns empty user model if user doesn't exist.
            return new object();
        }

        /// <summary>
        /// Grabs user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>user</returns>
        public async Task<UserModel> GetUserById(string userId)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Id, userId);
            return await _userCollection.Find(filter).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Grabs user id by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>if email exist returns user id else returns "User id could not be found!".</returns>
        public async Task<string> GetUserIDByEmail(string email)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Email, email);
            var projection = Builders<UserModel>.Projection.Include(u => u.Id);

            var user = await _userCollection.Find(filter).Project<UserModel>(projection).SingleOrDefaultAsync();

            if (user != null)
            {
                return user.Id;
            }
            return "User id could not be found!";
        }

        // Update user email
        public async Task<bool> UpdateUserEamil(string newEmail, string userId)
        {
            var Filter = Builders<UserModel>.Filter.Eq(u => u.Id, userId);
            var update = Builders<UserModel>.Update.Set(u => u.Email, newEmail);

            var result = await _userCollection.UpdateOneAsync(Filter, update);

            if (result.ModifiedCount > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes user by user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True: if deleted, False: if not deleted</returns>
        public async Task<bool> DeleteUser(string userId)
        {
            var userFilter = Builders<UserModel>.Filter.Eq(user => user.Id, userId);
            var deleteResult = await _userCollection.DeleteOneAsync(userFilter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        /// <summary>
        /// Hashes password by generating a salt and hashing the password.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Hashed password</returns>
        private static string HashPassword(string password)
        {
            // Generate a salt and hash the password
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        /// <summary>
        /// Authtinactes user's password when they login to the extsing password in the db.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        /// <returns>A boolean value if the password correctly matches the password in the db.</returns>
        private static bool VerifyPassword(string password, string hashedPassword)
        {
            // Verify the password against the hashed password
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
