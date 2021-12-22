using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Labb3Kompl.Model
{
    class User : ObservableObject
    {
        [BsonId]
        public ObjectId ObjectId { get; set; }

        [BsonElement]
        public string Username { get; set; }

        [BsonElement]
        public string Password { get; set; }

        [BsonElement]
        public Dictionary<Produkt, int> Kundkorg { get; set; }
        
        public override string ToString()
        {
            return $"Du är inloggad som: {Username}";
        }
    }
}
