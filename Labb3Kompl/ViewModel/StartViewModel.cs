using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Labb3Kompl.ViewModel
{
    class StartViewModel : ObservableObject
    {
        private NavigationManager _navigationManager;

        public StartViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            var users = BaseWindowViewModel.database.GetCollection<BsonDocument>("users");
            var admin = BaseWindowViewModel.database.GetCollection<BsonDocument>("admin");
        }
        
        [BsonId]
        private ObjectId ObjectId { get; set; }

        private User CurrentUser { get; set; }

        [BsonElement]
        public string Username { get; set; }

        [BsonElement]
        public string Password { get; set; }
    }
}
