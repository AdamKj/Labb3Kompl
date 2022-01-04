using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Labb3Kompl.ViewModel
{
    class KundProfilViewModel : ObservableObject
    {
        public ObservableCollection<Produkt> Products { get; set; } = new();
        public ObservableCollection<Produkt> UserCart { get; set; } = new();
        private NavigationManager _navigationManager;
        private UserManager _userManager;
        private User _currentUser;
        private Butik _butik = new();
        private readonly Managers.MongoDB _db = new("Butik");
        private IMongoDatabase _database;


        public ICommand StartViewCommand { get; }
        public ICommand ShopViewCommand { get; }
        public ICommand DeleteProductCommand => new RelayCommand(() => DeleteSelectedProduct(SelectedProduct));
        public ICommand ExitShopCommand => new RelayCommand(CheckOut);
        public KundProfilViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            CurrentUser = _userManager.CurrentUser;
            StartViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new StartViewModel(_navigationManager, _userManager); });
            ShopViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new ShopViewModel(_navigationManager, _userManager); });
            LoadProductsInUserCart();
            LoadProducts();
            ShowTotalSum();
        }

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        private Produkt _product;
        public Produkt SelectedProduct
        {
            get => _product;
            set
            {
                if (_product != value)
                {
                    _product = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                }
            }
        }

        private int _amountToRemove;

        public int AmountToRemove
        {
            get => _amountToRemove;
            set => SetProperty(ref _amountToRemove, value);
        }

        private string _totalSum;
        public string TotalSum
        {
            get => _totalSum;
            set => SetProperty(ref _totalSum, value);
        }

        public void LoadProductsInUserCart()
        {
            foreach (var item in _userManager.CurrentUser.Kundkorg)
            {
                UserCart.Add(item);
            }
        }
        public void DeleteSelectedProduct(Produkt produkt)
        {
            produkt = SelectedProduct;

            if (AmountToRemove == 0)
            {
                MessageBox.Show("Vänligen välj hur många varor du vill ta bort först.", "Error", MessageBoxButton.OK);
                return;
            }

            if (SelectedProduct == null)
            {
                MessageBox.Show("Vänligen välj vilken vara du vill ta bort först.", "Error", MessageBoxButton.OK);
                return;
            }

            var temp = Products.FirstOrDefault(p => p.ObjectId == SelectedProduct.ObjectId);
            if (SelectedProduct.Amount > AmountToRemove)
            {
                SelectedProduct.Amount -= AmountToRemove;
                temp.Amount += AmountToRemove;
                _userManager.CurrentUser.Kundkorg = UserCart;
                MessageBox.Show($"Du har tagit bort {AmountToRemove}st {SelectedProduct} från din kundkorg", "Success", MessageBoxButton.OK);
                _db.UpsertRecord("Users", _userManager.CurrentUser);
                _db.UpsertProduct("Produkter", temp);
                AmountToRemove = 0;
                CollectionViewSource.GetDefaultView(UserCart).Refresh();
                return;
            }

            temp.Amount += SelectedProduct.Amount;
            UserCart.Remove(UserCart.Single(p => p.ObjectId == SelectedProduct.ObjectId));
            _userManager.CurrentUser.Kundkorg = UserCart;
            MessageBox.Show("Produkten har blivit borttagen från kundvagnen!", "Success", MessageBoxButton.OK);
            _db.UpsertRecord("Users", _userManager.CurrentUser);
            _db.UpsertProduct("Produkter", temp);
            AmountToRemove = 0;
            CollectionViewSource.GetDefaultView(UserCart).Refresh();
        }

        public async void CheckOut()
        {
            await _butik.CheckOutUser(_userManager.CurrentUser);
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
        public  string ShowTotalSum()
        {
            double sum = 0;
            foreach (var item in UserCart)
            {
                sum += item.Amount * item.Price;
            }
            TotalSum = $"Din totala summa är: {sum}kr";
            return TotalSum;
        }
    }
}
