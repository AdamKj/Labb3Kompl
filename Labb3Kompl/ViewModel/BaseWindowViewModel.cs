using System.Windows.Input;
using Labb3Kompl.Managers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;

namespace Labb3Kompl.ViewModel
{
    class BaseWindowViewModel : ObservableObject
    {
        private readonly NavigationManager _navigationManager;

        public ObservableObject CurrentView => _navigationManager.CurrentView;

        public static MongoClient dbClient = new MongoClient(
            "mongodb+srv://nkt:adamkjellberg98@cluster0.zyo4q.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
        public static IMongoDatabase database = dbClient.GetDatabase("butik");

        public BaseWindowViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            StartViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new StartViewModel(_navigationManager); });
            KundprofilViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new KundProfilViewModel(_navigationManager); });
            AdminViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new AdminViewModel(_navigationManager); });
            ShopViewCommand = new RelayCommand(() => { _navigationManager.CurrentView = new ShopViewModel(_navigationManager); });
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
