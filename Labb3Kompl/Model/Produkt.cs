using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Labb3Kompl.Model
{
    class Produkt : ObservableObject
    {
        private readonly Managers.MongoDB _db = new("Butik");
        private readonly IMongoDatabase _database;

        [BsonId]
        private ObjectId ObjectId { get; set; }

        [BsonElement]
        public string ProductName { get; set; }

        [BsonElement]
        public string ProductType { get; set; }

        [BsonElement]
        public double Price { get; set; }

        public Produkt()
        {
            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase("Butik");
            _database.GetCollection<Managers.MongoDB>("Produkter");
            //_db.InsertNew("Produkter", new { ProductName = "Strumpor", ProductType = "Kläder", Price = 69 });
        }

        public void AddProducts()
        {
        }
    }
}
