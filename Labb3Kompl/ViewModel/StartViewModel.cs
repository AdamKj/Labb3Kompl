using Labb3Kompl.Managers;
using Labb3Kompl.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;
using System;
using System.Windows;
using System.Windows.Input;

namespace Labb3Kompl.ViewModel
{
    class StartViewModel : ObservableObject
    {
        private readonly NavigationManager _navigationManager;
        private string _username;
        private string _newUsername;
        private string _password;
        private string _newPassword;
        private readonly Managers.MongoDB _db = new("Butik"); 
        private readonly IMongoDatabase _database;
        private readonly UserManager _userManager;

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
                CheckTypeOfUser();
                LogInExistingUser(CurrentUser);
                LogInAdmin();
            });
        } }

        public StartViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase("Butik");
            _database.GetCollection<Managers.MongoDB>("Users");
            _database.GetCollection<Managers.MongoDB>("Admin");
            _navigationManager = navigationManager;
            _userManager = userManager;
            AddNewUserCommand = new RelayCommand(AddNewUser, CanAddNewUser);
            CurrentUser = _userManager.CurrentUser;
        }

        /// <summary>
        /// Inserts a new User into the User collection
        /// </summary>
        public void AddNewUser()
        {
            while (true)
            {
                if (String.IsNullOrWhiteSpace(NewUsername) || String.IsNullOrWhiteSpace(NewPassword))
                {
                    MessageBox.Show("Användarnamn och Lösenord kan inte vara tomt. Försök igen.", "Error", MessageBoxButton.OK);
                    return;
                }

                break;
            }

            _db.InsertNew("Users", new User { Username = NewUsername, Password = NewPassword });
            MessageBox.Show("Användaren är nu skapad! Vänligen logga in.", "Success", MessageBoxButton.OK);
            NewUsername = null;
            NewPassword = null;
        }

        /// <summary>
        /// Checks if the said Username already exists or not
        /// </summary>
        /// <returns></returns>
        public bool CanAddNewUser()
        {
            var collection = _database.GetCollection<User>("Users");
            bool exists =  collection.Find(u => u.Username == NewUsername).Any();

            if (exists)
            {
                MessageBox.Show("Det här användarnamnet finns redan. Vänligen logga in befintlig användare eller försök med ett annat användarnamn.", "Error", MessageBoxButton.OK);
                NewUsername = null;
                NewPassword = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Search through the User collection in the DB to find said Username. If it exists, you get logged in.
        /// </summary>
        /// <param name="currentUser"></param>
        public void LogInExistingUser(User currentUser)
        {
            
            var collection = _database.GetCollection<User>("Users");
            bool usernameExists = collection.Find(u => u.Username == Username).Any();
            //bool passwordExists = collection.Find(u => u.Password == Password).Equals(Username);

            if (usernameExists)
            {
                CurrentUser = collection.Find(u => u.Username == Username).Single();
                _userManager.CurrentUser = CurrentUser;

                if (Password == _userManager.CurrentUser.Password)
                {
                    MessageBox.Show($"Du har loggat in som {Username}!", "Success", MessageBoxButton.OK);
                    _navigationManager.CurrentView = new KundProfilViewModel(_navigationManager, _userManager);
                    return;
                }

                MessageBox.Show("Fel lösenord. Försök igen.", "Error", MessageBoxButton.OK);
                Password = null;
            }
        }

        /// <summary>
        /// Search through the Admin collection in the DB to find said Username. If it exists, you log in as an Admin.
        /// </summary>
        public void LogInAdmin()
        {
            var adminCollection = _database.GetCollection<User>("Admin");
            bool adminExists = adminCollection.Find(u => u.Username == Username).Any();

            if (adminExists)
            {
                MessageBox.Show($"Du har loggat in som {Username}!", "Admin", MessageBoxButton.OK);
                Username = null;
                Password = null;
                _navigationManager.CurrentView = new AdminViewModel(_navigationManager);
            }
        }

        /// <summary>
        /// Checks if the said Username is an Admin or a regular User
        /// </summary>
        public void CheckTypeOfUser()
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
