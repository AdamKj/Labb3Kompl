﻿using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Labb3Kompl.ViewModel
{
    class KundProfilViewModel : ObservableObject
    {
        public ObservableCollection<Produkt> UserCart { get; set; } = new();
        private NavigationManager _navigationManager;
        private UserManager _userManager;
        private User _currentUser;
        private Butik _butik = new(new UserManager());


        public ICommand StartViewCommand { get; }
        public ICommand ShopViewCommand { get; }
        public ICommand ExitShopCommand => new RelayCommand(_butik.CheckOut);
        public KundProfilViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            CurrentUser = _userManager.CurrentUser;
            StartViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new StartViewModel(_navigationManager, _userManager); });
            ShopViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new ShopViewModel(_navigationManager, _userManager); });
            LoadProductsInUserCart();
        }

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
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

        public void LoadProductsInUserCart()
        {
            foreach (var item in _userManager.CurrentUser.Kundkorg)
            {
                UserCart.Add(item);
            }
        }
    }
}
