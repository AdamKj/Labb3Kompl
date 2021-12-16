using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public void InsertNewUser<T>(string dbCollection, T input)
        {
            var collection = _database.GetCollection<T>(dbCollection);
            collection.InsertOneAsync(input);
        }
    }
}
