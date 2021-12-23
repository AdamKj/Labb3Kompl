using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Labb3Kompl.ViewModel
{
    class KundProfilViewModel : ObservableObject
    {
        public ObservableCollection<Produkt> Kundkorg { get; set; } = new();
        private NavigationManager _navigationManager;
        private UserManager _userManager;
        private User _currentUser;


        public ICommand StartViewCommand { get; }
        public ICommand ShopViewCommand { get; }
        public KundProfilViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            CurrentUser = _userManager.CurrentUser;
            StartViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new StartViewModel(_navigationManager, _userManager); });
            ShopViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new ShopViewModel(_navigationManager, _userManager); });

        }

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        private int _amount;

        public int Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        private Produkt _produkt;
        public Produkt SelectedProdukt
        {
            get => _produkt;
            set
            {
                if (_produkt != value)
                {
                    _produkt = value;
                    OnPropertyChanged(nameof(SelectedProdukt));
                }
            }
        }
    }
}
