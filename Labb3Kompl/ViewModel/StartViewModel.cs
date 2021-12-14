using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Labb3Kompl.ViewModel
{
    class StartViewModel : ObservableObject
    {
        private NavigationManager _navigationManager;

        public StartViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        private User CurrentUser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
