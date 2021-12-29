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
        private readonly Managers.MongoDB _db = new("Butik");
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
            user.Kundkorg.Clear();
            await _db.UpsertRecord("Users", _userManager.CurrentUser);
            MessageBox.Show("Tack för att du handlade i butiken!", "Goodbye", MessageBoxButton.OK);
            Application.Current.Shutdown();
        }
    }
}
