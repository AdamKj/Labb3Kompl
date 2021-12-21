using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Labb3Kompl.ViewModel
{
    class ShopViewModel : ObservableObject
    {
        public ObservableCollection<Produkt> Produkter { get; set; } = new();
        private NavigationManager _navigationManager;
        private UserManager _userManager;
        private Butik _currentStore;
        private Produkt _produkt;
        private IMongoDatabase _database;
        private IMongoCollection<Produkt> _collection;

        public ShopViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
        }

        public Butik CurrentStore { get; set; }
        public User CurrentUser { get; set; }

        public Produkt Produkt
        {
            get => _produkt;
            set
            {
                _collection = _database.GetCollection<Produkt>("Produkter");
                var documents = _collection.Find(new BsonDocument()).ToList();
            }
        }
    }
}
