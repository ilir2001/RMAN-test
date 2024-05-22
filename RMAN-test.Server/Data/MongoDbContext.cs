using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using RMAN_test.Server.Models;

namespace RMAN_test.Server.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        // Define your collections here
        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("posts");
        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
        // Add more collections as needed

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
    }
}
