using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Labb3Kompl.ViewModel
{
    class KundProfilViewModel : ObservableObject
    {
        private NavigationManager navigationManager;
        private readonly User _currentUser;

        public KundProfilViewModel(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
            _currentUser = new User();
        }

        public string CurrentUser
        {
            get => _currentUser.ToString();
        }
    }
}
