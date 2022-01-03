using Labb3Kompl.Managers;
using System.Threading.Tasks;
using System.Windows;

namespace Labb3Kompl.Model
{
    class Butik
    {
        private UserManager _userManager = new();
        private readonly Managers.MongoDB _db = new("Butik");

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
