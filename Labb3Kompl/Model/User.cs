using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.ObjectModel;

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
        public ObservableCollection<Produkt> Kundkorg { get; set; }

        public User()
        {
            Kundkorg = new ObservableCollection<Produkt>();
        }
        
        public override string ToString()
        {
            return $"Du är inloggad som: {Username}";
        }
    }
}
