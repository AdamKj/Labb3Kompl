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

        public void UpsertRecord<T>(string dbCollection, ObjectId id, T record)
        {
            var collection = _database.GetCollection<T>(dbCollection);
            var result = collection.ReplaceOneAsync(
                new BsonDocument("_id", id),
                record,
                new UpdateOptions { IsUpsert = true });
        }

        public void DeleteRecord<T>(string dbCollection, ObjectId id)
        {
            var collection = _database.GetCollection<T>(dbCollection);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOneAsync(filter);
        }
    }
}
