namespace BuggyBuddyAPI.Models
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DataBaseName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
        public string CartCollectionName { get; set; } = null!;
    }
}
