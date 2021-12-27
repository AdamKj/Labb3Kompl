using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Labb3Kompl.ViewModel
{
    class ShopViewModel : ObservableObject
    {
        public ObservableCollection<Produkt> Produkter { get; set; } = new();
        public ObservableCollection<Produkt> UserCart { get; set; } = new();
        private readonly Managers.MongoDB _db = new("Butik");
        private NavigationManager _navigationManager;
        private UserManager _userManager = new();
        private User _currentUser;
        private Butik _currentStore;
        private Produkt _produkt;
        private IMongoDatabase _database;
        private IMongoCollection<Produkt> _collection;
        public ICommand KundprofilViewCommand { get; }
        public ICommand AddToCartCommand => new RelayCommand(AddToCart);

        public ShopViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            LoadProducts();
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
                Produkter.Add(item);
            }
        }
    }
}
