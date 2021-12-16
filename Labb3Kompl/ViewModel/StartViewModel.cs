using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Labb3Kompl.ViewModel
{
    class StartViewModel : ObservableObject
    {
        private NavigationManager _navigationManager;
        private string _username;
        private string _newUsername;
        private string _password;
        private readonly Managers.MongoDB _db = new Managers.MongoDB("Butik"); 
        private readonly IMongoDatabase _database;
        
        private User CurrentUser { get; set; }

        [BsonId]
        private ObjectId ObjectId { get; set; }

        public string NewUsername
        {
            get => _newUsername;
            set => SetProperty(ref _newUsername, value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }
        
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand AddNewUserCommand { get; }

        public StartViewModel(NavigationManager navigationManager)
        {
            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase("Butik");
            _database.GetCollection<Managers.MongoDB>("Users");
            _navigationManager = navigationManager;
            AddNewUserCommand = new RelayCommand(AddNewUser, CanAddNewUser);
        }

        public StartViewModel()
        {
            
        }

        public void AddNewUser()
        {
            _db.InsertNewUser("Users", new StartViewModel { Username = NewUsername, Password = Password });
            MessageBox.Show("Användaren är nu skapad!", "Success", MessageBoxButton.OK);
            NewUsername = null;
            Password = null;
        }

        public bool CanAddNewUser()
        {
            var collection = _database.GetCollection<StartViewModel>("Users");
            bool exists =  collection.Find(svm => svm.Username == NewUsername).Any();

            if (exists)
            {
                MessageBox.Show("Det här användarnamnet finns redan. Vänligen logga in befintlig användare", "Error", MessageBoxButton.OK);
                NewUsername = null;
                Password = null;
                return false;
            }
            
            return true;
        }
    }

}
