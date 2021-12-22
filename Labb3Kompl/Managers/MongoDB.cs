using System.Threading.Tasks;
using Labb3Kompl.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Labb3Kompl.Managers
{
    class MongoDB
    {

        private IMongoDatabase _database;
        private User user;

        public MongoDB(string database)
        {
            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase(database);
        }
        public void InsertNew<T>(string dbCollection, T input)
        {
            var collection = _database.GetCollection<T>(dbCollection);
            collection.InsertOneAsync(input);
        }

        public Task UpsertRecord(string dbCollection, User user)
        {
            var collection = _database.GetCollection<User>(dbCollection);
            var filter = Builders<User>.Filter.Eq("_id", user.ObjectId);
            return collection.ReplaceOneAsync(filter, user, new ReplaceOptions {IsUpsert = true});
        }

        public void DeleteRecord<T>(string dbCollection, ObjectId id)
        {
            var collection = _database.GetCollection<T>(dbCollection);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOneAsync(filter);
        }
    }
}
