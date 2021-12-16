using System.Windows;
using Labb3Kompl.Managers;
using Labb3Kompl.ViewModel;
using MongoDB.Driver;
using MongoDB = Labb3Kompl.Managers.MongoDB;

namespace Labb3Kompl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;

        public App()
        {
            _navigationManager = new NavigationManager();
        }

        private IMongoDatabase _database;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _navigationManager.CurrentView = new StartViewModel(_navigationManager);
            var mainWindow = new BaseWindowViewModel(_navigationManager);
            var startUpWindow = new MainWindow { DataContext = mainWindow };

            startUpWindow.Show();
        }
    }
}
