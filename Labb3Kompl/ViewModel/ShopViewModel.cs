using System;
using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MongoDB.Bson;
using MongoDB.Driver.Core.Clusters.ServerSelectors;

namespace Labb3Kompl.ViewModel
{
    class ShopViewModel : ObservableObject
    {
        public ObservableCollection<Produkt> Products { get; set; } = new();
        public ObservableCollection<string> ProductType { get; set; } = new();
        public ObservableCollection<Produkt> UserCart { get; set; } = new();
        private readonly Managers.MongoDB _db = new("Butik");
        private NavigationManager _navigationManager;
        private UserManager _userManager = new();
        private Produkt _produkt;
        private IMongoDatabase _database;
        public ICommand KundprofilViewCommand { get; }
        public ICommand AddToCartCommand => new RelayCommand(AddToCart);

        public ShopViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            LoadProducts();
            GetProductTypes();
            LoadProductType();
            _userManager.CurrentUser.Kundkorg = UserCart;
            _navigationManager = navigationManager;
            _userManager = userManager;
            KundprofilViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new KundProfilViewModel(_navigationManager, _userManager); });
        }
        

        private int _amount;
        public int Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public ICollectionView ItemsView
        {
            get => CollectionViewSource.GetDefaultView(Products);
        }

        private string _selectedProductType;
        public string SelectedProductType
        {
            get => _selectedProductType;
            set
            {
                _selectedProductType = value;
                OnPropertyChanged();
                ItemsView.Refresh();
            }
        }

        public Produkt SelectedProduct
        {
            get => _produkt;
            set
            {
                if (_produkt != value)
                {
                    _produkt = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                }
            }
        }

        public void AddToCart()
        {
            if (Amount == 0 || SelectedProduct == null)
            {
                MessageBox.Show("Vänligen välj produkt och antalet du vill lägga i din kundkorg","Error", MessageBoxButton.OK);
                return;
            }
            if (UserCart.Contains(SelectedProduct))
            {
                MessageBox.Show($"Du har lagt till {Amount}st till {SelectedProduct} i din kundkorg");
                _userManager.CurrentUser.Kundkorg = UserCart;
                _produkt.Amount += Amount;
                _db.UpsertRecord("Users", _userManager.CurrentUser);
                Amount = 0;
                return;
            }
            
            UserCart.Add(SelectedProduct);
            MessageBox.Show($"Du har lagt till {Amount}st {SelectedProduct} i din kundkorg");
            _userManager.CurrentUser.Kundkorg = UserCart;
            _produkt.Amount = Amount;
            Amount = 0;
            _db.UpsertRecord("Users", _userManager.CurrentUser);
        }

        /// <summary>
        ///Loads in all data from collection into the listview
        /// </summary>
        public void LoadProducts()
        {
            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase("Butik");
            var collection = _database.GetCollection<Produkt>("Produkter").AsQueryable().ToList();

            foreach (var item in collection)
            {
                Products.Add(item);
            }
        }

        public void GetProductTypes()
        {
            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase("Butik");
            var collection = _database.GetCollection<Produkt>("Produkter");
            
            var productTypes = collection.AsQueryable().Select(e => e.ProductType).Distinct();

            foreach (var items in productTypes)
            {
                ProductType.Add(items);
            }
        }

        public void LoadProductType()
        {
            var source = CollectionViewSource.GetDefaultView(Products);
            source.Filter = o => Filter(o as Produkt);
        }

        private bool Filter(Produkt type)
        {
            return SelectedProductType == null
                || type.ProductType.IndexOf(SelectedProductType, StringComparison.OrdinalIgnoreCase) != -1;
        }
    }
}
