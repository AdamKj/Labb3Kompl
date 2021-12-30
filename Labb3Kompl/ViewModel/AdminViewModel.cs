using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;

namespace Labb3Kompl.ViewModel
{
    class AdminViewModel : ObservableObject
    {
        public ObservableCollection<Produkt> Products { get; set; } = new();
        private readonly NavigationManager _navigationManager;
        private readonly UserManager _userManager;
        private Produkt _produkt;
        private IMongoDatabase _database;
        private readonly Managers.MongoDB _db = new("Butik");

        public ICommand StartViewCommand { get; }
        public ICommand AddNewProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public AdminViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            LoadProducts();
            _navigationManager = navigationManager;
            _userManager = userManager;
            CurrentUser = _userManager.CurrentUser;
            StartViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new StartViewModel(_navigationManager, _userManager); });
            AddNewProductCommand = new RelayCommand(AddNewProduct);
            DeleteProductCommand = new RelayCommand(() => DeleteSelectedProduct(SelectedProduct));
        }
        public User CurrentUser { get; set; }

        private string _productName;
        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }

        private string _productType;

        public string ProductType
        {
            get => _productType;
            set => SetProperty(ref _productType, value);
        }

        private double _price;
        public double Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        private int _amount;

        public int Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
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
        public void AddNewProduct()
        {
            while (true)
            {
                if (String.IsNullOrWhiteSpace(ProductName) || String.IsNullOrWhiteSpace(ProductType) || Price == 0 || Amount == 0)
                {
                    MessageBox.Show("Vänligen fyll i alla fält för att lägga till produkt.", "Error", MessageBoxButton.OK);
                    return;
                }

                break;
            }

            _db.InsertNew("Produkter", new Produkt { ProductName = ProductName, ProductType = ProductType, Amount = Amount, Price = Price});
            MessageBox.Show("Produkten är nu tillagd i butiken!", "Success", MessageBoxButton.OK); 
            Products.Clear();
            LoadProducts();
            ProductName = null;
            ProductType = null;
            Amount = 0;
            Price = 0;
        }

        public void DeleteSelectedProduct(Produkt produkt)
        {
            SelectedProduct = produkt;

            if (SelectedProduct == null)
            {
                MessageBox.Show("Vänligen välj produkten du vill ta bort.", "Error", MessageBoxButton.OK);
                return;
            }

            MessageBox.Show("Produkten har blivit borttagen från butiken!", "Success", MessageBoxButton.OK);
            Products.Remove(Products.Single(p => p.ObjectId == _produkt.ObjectId));
            _db.DeleteRecord<Produkt>("Produkter", produkt.ObjectId);
            CollectionViewSource.GetDefaultView(Products).Refresh();
        }
    }
}
