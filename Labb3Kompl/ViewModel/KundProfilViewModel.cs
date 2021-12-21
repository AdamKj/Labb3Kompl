using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace Labb3Kompl.ViewModel
{
    class KundProfilViewModel : ObservableObject
    {
        public Dictionary<Produkt, int> Kundkorg { get; set; } = new();
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

        public User UserKundkorg
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                SetProperty(ref _currentUser, value);
                foreach (var item in Kundkorg)
                {
                    CurrentUser.Kundkorg.Add(item.Key, item.Value);
                }
            }
        }
    }
}
