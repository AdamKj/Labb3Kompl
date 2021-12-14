using System.Windows;
using Labb3Kompl.Managers;
using Labb3Kompl.ViewModel;

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
