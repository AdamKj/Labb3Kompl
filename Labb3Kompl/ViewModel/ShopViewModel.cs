using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Labb3Kompl.ViewModel
{
    class ShopViewModel : ObservableObject
    {
        private NavigationManager navigationManager;

        public ShopViewModel(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
        }

        private Butik CurrentStore { get; set; }
        private User CurrentUser { get; set; }
    }
}
