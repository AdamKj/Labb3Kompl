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
        private string _newPassword;
        private readonly Managers.MongoDB _db = new Managers.MongoDB("Butik"); 
        private readonly IMongoDatabase _database;
        
        private User CurrentUser { get; set; }
        
        public string NewUsername
        {
            get => _newUsername;
            set => SetProperty(ref _newUsername, value);
        }
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
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
        public ICommand LogInExistingUserCommand { get
        {
            return new RelayCommand(() =>
            {
                LogInExistingUser(CurrentUser);
                LogInAdmin();
                CheckIfUserOrAdmin();
            });
        } }

        public StartViewModel(NavigationManager navigationManager)
        {
            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase("Butik");
            _database.GetCollection<Managers.MongoDB>("Users");
            _database.GetCollection<Managers.MongoDB>("Admin");
            _navigationManager = navigationManager;
            AddNewUserCommand = new RelayCommand(AddNewUser, CanAddNewUser);

        }

        public void AddNewUser()
        {
            _db.InsertNewUser("Users", new User { Username = NewUsername, Password = NewPassword });
            MessageBox.Show("Användaren är nu skapad! Vänligen logga in.", "Success", MessageBoxButton.OK);
            NewUsername = null;
            Password = null;
        }

        public bool CanAddNewUser()
        {
            var collection = _database.GetCollection<User>("Users");
            bool exists =  collection.Find(u => u.Username == NewUsername).Any();

            if (exists)
            {
                MessageBox.Show("Det här användarnamnet finns redan. Vänligen logga in befintlig användare eller försök med ett annat användarnamn.", "Error", MessageBoxButton.OK);
                NewUsername = null;
                Password = null;
                return false;
            }
            
            return true;
        }

        public void LogInExistingUser(User currentUser)
        {
            
            var collection = _database.GetCollection<User>("Users");
            bool exists = collection.Find(u => u.Username == Username).Any();

            if (exists)
            {
                CurrentUser = currentUser;
                MessageBox.Show($"Du har loggat in som {Username}!","Success", MessageBoxButton.OK);
                NewUsername = null;
                Password = null;
                _navigationManager.CurrentView = new KundProfilViewModel(_navigationManager);
            }
        }

        public void LogInAdmin()
        {
            var adminCollection = _database.GetCollection<User>("Admin");
            bool adminExists = adminCollection.Find(u => u.Username == Username).Any();

            if (adminExists)
            {
                MessageBox.Show($"Du har loggat in som {Username}!", "Admin", MessageBoxButton.OK);
                NewUsername = null;
                Password = null;
                _navigationManager.CurrentView = new AdminViewModel(_navigationManager);
            }
        }

        public void CheckIfUserOrAdmin()
        {
            var collection = _database.GetCollection<User>("Users");
            bool exists = collection.Find(u => u.Username == Username).Any();
            var adminCollection = _database.GetCollection<User>("Admin");
            bool adminExists = adminCollection.Find(u => u.Username == Username).Any();

            if (!adminExists && !exists)
            {
                MessageBox.Show(
                    "Det här användarnamnet finns inte. Vänligen försök igen eller registrera ny användare.", "Error",
                    MessageBoxButton.OK);
                Username = null;
                Password = null;
            }
        }
    }

}
