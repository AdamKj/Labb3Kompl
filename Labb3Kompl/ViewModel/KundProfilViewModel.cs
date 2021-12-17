using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Labb3Kompl.ViewModel
{
    class KundProfilViewModel : ObservableObject
    {
        private NavigationManager navigationManager;

        public KundProfilViewModel(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
        }
    }
}
