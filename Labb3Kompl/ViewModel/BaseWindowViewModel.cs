using Labb3Kompl.Managers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace Labb3Kompl.ViewModel
{
    class BaseWindowViewModel : ObservableObject
    {
        private readonly NavigationManager _navigationManager;
        private UserManager _userManager;

        public ObservableObject CurrentView => _navigationManager.CurrentView;

        
        public BaseWindowViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            StartViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new StartViewModel(_navigationManager, _userManager); });
            KundprofilViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new KundProfilViewModel(_navigationManager, _userManager); });
            AdminViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new AdminViewModel(_navigationManager, _userManager); });
            ShopViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new ShopViewModel(_navigationManager, _userManager); });
            _navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;
        }

        public ICommand StartViewCommand { get; }
        public ICommand KundprofilViewCommand { get; }
        public ICommand AdminViewCommand { get; }
        public ICommand ShopViewCommand { get; }

        private void CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentView));
        }
    }
}
