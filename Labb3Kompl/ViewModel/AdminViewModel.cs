using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Labb3Kompl.ViewModel
{
    class AdminViewModel : ObservableObject
    {
        private NavigationManager navigationManager;

        public AdminViewModel(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
        }

        private User CurrentUser { get; set; }
    }
}
