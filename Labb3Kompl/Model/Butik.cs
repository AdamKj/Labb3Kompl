using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Labb3Kompl.Managers;

namespace Labb3Kompl.Model
{
    class Butik
    {
        private UserManager _userManager;
        public string Username { get; set; }
        public User Admin { get; set; }

        public Dictionary<Produkt, int> Lager;

        public bool LogInAdmin(User user)
        {
            return true;
        }

        public Butik(UserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task CheckOutUser(User user)
        {
            _userManager.CurrentUser.Kundkorg.Clear();
            MessageBox.Show("Tack för att du handlade i butiken!", "Goodbye", MessageBoxButton.OK);
            Application.Current.Shutdown();
        }
        public async void CheckOut()
        {
            await CheckOutUser(_userManager.CurrentUser);
        }
    }
}
