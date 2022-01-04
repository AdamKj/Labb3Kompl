using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

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
            _navigationManager = navigationManager;
            _userManager = userManager;
            LoadProducts();
            GetProductTypes();
            LoadProductType();
            UserCart = _userManager.CurrentUser.Kundkorg;
            KundprofilViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new KundProfilViewModel(_navigationManager, _userManager); });
            ImageDecider();
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
                    ImageUrl = _produkt.ImageUrl;
                }
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        public void AddToCart()
        {
            if (SelectedProduct.Amount == 0 || Amount > SelectedProduct.Amount)
            {
                MessageBox.Show("Den här varan är tyvärr slut eller finns för få av i lagret. Kontakta butiksadministratör", "Error", MessageBoxButton.OK);
                Amount = 0;
                return;
            }
            if (Amount == 0 || SelectedProduct == null)
            {
                MessageBox.Show("Vänligen välj produkt och antalet du vill lägga i din kundkorg","Error", MessageBoxButton.OK);
                return;
            }

            var existingProduct = UserCart.FirstOrDefault(p => p.ObjectId == SelectedProduct.ObjectId);
            if (existingProduct != null)
            {
                MessageBox.Show($"Du har lagt till {Amount}st mer {SelectedProduct} i din kundkorg");
                _userManager.CurrentUser.Kundkorg = UserCart;
                existingProduct.Amount += Amount;
                _produkt.Amount -= Amount;
                _db.UpsertRecord("Users", _userManager.CurrentUser);
                _db.UpsertProduct("Produkter", SelectedProduct);
                Amount = 0;
                return;
            }

            var produktToAdd = SelectedProduct.Copy();
            produktToAdd.Amount = Amount;
            UserCart.Add(produktToAdd);
            _produkt.Amount -= Amount;
            MessageBox.Show($"Du har lagt till {Amount}st {SelectedProduct} i din kundkorg");
            _userManager.CurrentUser.Kundkorg = UserCart;
            _db.UpsertRecord("Users", _userManager.CurrentUser);
            _db.UpsertProduct("Produkter", SelectedProduct);
             Amount = 0;
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
        public void ImageDecider()
        {
            
        }
    }
}
