using MongoDB.Driver;

namespace Labb3Kompl.Managers
{
    class MongoDB
    {

        private IMongoDatabase _database;

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
    }
}
