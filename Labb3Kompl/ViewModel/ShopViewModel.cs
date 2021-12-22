using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Labb3Kompl.ViewModel
{
    class ShopViewModel : ObservableObject
    {
        public ObservableCollection<Produkt> Produkter { get; set; } = new();
        public Dictionary<Produkt, int> Kundkorg { get; set; } = new();
        private readonly Managers.MongoDB _db = new("Butik");
        private NavigationManager _navigationManager;
        private UserManager _userManager = new();
        private User _currentUser;
        private Butik _currentStore;
        private Produkt _produkt;
        private IMongoDatabase _database;
        private IMongoCollection<Produkt> _collection;
        public ICommand AddToCartCommand => new RelayCommand(AddToCart);

        public ShopViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            LoadProducts();
            _navigationManager = navigationManager;
            _userManager = userManager;
        }

        public Butik CurrentStore { get; set; }
        public User CurrentUser { get; set; }

        private int _amount;

        public int Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

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

        public void AddToCart()
        {
            int val;
            if (Kundkorg.TryGetValue(SelectedProdukt, out val))
            {
                Kundkorg[SelectedProdukt] = val += Amount;
                return;
            }

            Kundkorg.Add(SelectedProdukt, Amount);
            MessageBox.Show($"Du har lagt till {Amount}st {SelectedProdukt} i din kundkorg");
            Amount = 0;
            _db.UpsertRecord("Users", _userManager.CurrentUser);
        }

        public void LoadProducts()
        {
            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase("Butik");
            var collection = _database.GetCollection<Produkt>("Produkter").AsQueryable().ToList();

            foreach (var item in collection)
            {
                Produkter.Add(item);
            }
        }
    }
}
