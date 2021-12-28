using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Labb3Kompl.Managers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Labb3Kompl.Model
{
    class Butik
    {
        private UserManager _userManager = new();
        private Produkt _produkt = new();
        private IMongoDatabase _database;
        public string Username { get; set; }
        public User Admin { get; set; }

        public Dictionary<Produkt, int> Lager;

        public bool LogInAdmin(User user)
        {
            return true;
        }

        public async Task CheckOutUser(User user)
        {
            _userManager.CurrentUser = user;
            //user.Kundkorg.Clear();
            ClearCart();
            MessageBox.Show("Tack för att du handlade i butiken!", "Goodbye", MessageBoxButton.OK);
            Application.Current.Shutdown();
        }

        public void ClearCart()
        {
            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase("Butik");
            var collection = _database.GetCollection<User>("Users");

            //var update = Builders<User>.Update.PullFilter(p => p.Kundkorg, f => f.ObjectId == _produkt.ObjectId);
            //var result = collection.FindOneAndUpdateAsync(p => p.ObjectId == _userManager.CurrentUser.ObjectId, update)
            //    .Result;

            var update = Builders<User>.Update.PullFilter(x => x.Kundkorg, Builders<Produkt>.Filter.Eq(x => x.ObjectId, _produkt.ObjectId));
            collection.FindOneAndUpdateAsync(x => x.ObjectId.Equals(_userManager.CurrentUser.ObjectId), update);
        }
    }
}
