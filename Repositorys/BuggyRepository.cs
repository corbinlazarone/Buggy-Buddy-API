using BuggyBuddyAPI.Contracts;
using BuggyBuddyAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System.ComponentModel;
using System.Web;
using ZstdSharp.Unsafe;

namespace BuggyBuddyAPI.Repositorys
{
    public class BuggyRepository : IBuggyContract
    {
        private readonly IMongoCollection<BuggyModel> _buggyCollection;

        public BuggyRepository(IOptions<DatabaseSettings> _databaseSettings)
        {
            var mongoClient = new MongoClient(
              _databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                _databaseSettings.Value.DataBaseName);

            _buggyCollection = mongoDatabase.GetCollection<BuggyModel>(
                _databaseSettings.Value.CartCollectionName);
        }


        // Test Connection
        public async Task<List<BuggyModel>> GetAllBuggys()
        {
            var result = await _buggyCollection.Find(_ => true).ToListAsync();
            return result;
        }


        /// <summary>
        /// Grabs buggy urls by user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>If the buggys exist returns buggys else returns a empy list.</returns>
        public async Task<List<BuggyModel>> GetUserBuggyUrls(string userId)
        {
            var buggyFilter = Builders<BuggyModel>.Filter.Eq(buggy => buggy.UserId, userId);
            var userBuggys = await _buggyCollection.Find(buggyFilter).ToListAsync();

            if (userBuggys.Count == 0)
            {
                return new List<BuggyModel>();
            }

            return userBuggys;
        }

        /// <summary>
        /// Adds the buggy url to the approiate user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="url"></param>
        /// <returns>returns "Buggy url successfully added!" if buggy was added. And "Maximum limit of 8 Buggy urls reached for this user" if that max buggys was reached.</returns>
        public async Task<string> AddBuggyUrlToUser(string userId, string url)
        {
            var BuggyUrlCount = _buggyCollection.CountDocuments(b => b.UserId == userId);
             
            if (BuggyUrlCount < 8)
            {
                if (!IsUrlValid(url))
                {
                    var buggy = new BuggyModel
                    {
                        UserId = (userId),
                        BuggyUrl = url,
                    };

                   await _buggyCollection.InsertOneAsync(buggy);

                    return "Buggy url successfully added!";
                }
                else
                {
                    return "Buggy url not valid.";
                }
            }
            else
            {
                return "Maximum limit of 8 Buggy urls reached for this user.";
            }
        }

        /// <summary>
        /// Deletes the specfic buggy connected with the correct buggyId and userId.
        /// </summary>
        /// <param name="buggyId"></param>
        /// <param name="userId"></param>
        /// <returns>A boolean value if the action was ackowleged or not and deleted.</returns>
        public async Task<bool> DeleteBuggy(string buggyId, string userId)
        {
            var buggyFilter = Builders<BuggyModel>.Filter.And(
                Builders<BuggyModel>.Filter.Eq(buggy => buggy.Id, buggyId),
                Builders<BuggyModel>.Filter.Eq(user => user.UserId, userId)
            );

            var deleteResult = await _buggyCollection.DeleteOneAsync(buggyFilter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }


        /// <summary>
        /// Checks if url is valid.
        /// </summary>
        /// <param name="urlString"></param>
        /// <returns>returns true or false.</returns>
        private bool IsUrlValid(string urlString)
        {
            string encodedUrl = HttpUtility.UrlEncode(urlString);
            return Uri.IsWellFormedUriString(encodedUrl, UriKind.Absolute);
        }
    }
}
