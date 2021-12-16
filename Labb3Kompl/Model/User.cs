using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb3Kompl.Model
{
    class User
    {

        [BsonId]
        private ObjectId ObjectId { get; set; }

        [BsonElement]
        public string Name { get; set; }

        [BsonElement]
        public string Password { get; set; }

        public Dictionary<Produkt, int> Kundkorg;
    }
}
