using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TrackerLibrary;
using TrackerWPFUI.Stores;
using TrackerWPFUI.ViewModels;
using TrackerWPFUI.Views;

namespace TrackerWPFUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public App()
        {
            _navigationStore = new NavigationStore();
            _modalNavigationStore = new ModalNavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            GlobalConfig.InitiallizeConnection(DatabaseType.TextFile, InitializeConfiguration());
            _navigationStore.CurrentViewModel = new DashBoardViewModel(_navigationStore, _modalNavigationStore);

            Window mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel(mainWindow, _navigationStore, _modalNavigationStore);
            MainWindow = mainWindow;
            MainWindow.Show();
            base.OnStartup(e);
        }

        private IConfiguration InitializeConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }
    }
}
