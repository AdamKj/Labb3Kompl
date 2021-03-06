using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Labb3Kompl.Model
{
    class Produkt : ObservableObject
    {
        private readonly IMongoDatabase _database;

        [BsonId]
        public ObjectId ObjectId { get; set; }

        [BsonElement]
        public string ProductName { get; set; }

        [BsonElement]
        public string ProductType { get; set; }

        [BsonElement]
        public double Price { get; set; }

        [BsonElement] 
        public int Amount { get; set; }

        [BsonElement]
        public string ImageUrl { get; set; }

        public Produkt()
        {
            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase("Butik");
            _database.GetCollection<Managers.MongoDB>("Produkter");
        }

        public override string ToString()
        {
            return $"{ProductName}";
        }

        public Produkt Copy()
        {
            var prod = new Produkt();
            prod.ObjectId = ObjectId;
            prod.ProductName = ProductName;
            prod.ProductType = ProductType;
            prod.Price = Price;
            prod.Amount = 0;
            prod.ImageUrl = ImageUrl;
            return prod;
        }
    }
}
