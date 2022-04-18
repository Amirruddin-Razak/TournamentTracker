using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
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
using TrackerWPFUI.ViewModels.Base;
using TrackerWPFUI.Views;

namespace TrackerWPFUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalNavigationStore>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton(s => new MainViewModel
            (
                s.GetRequiredService<NavigationStore>(),
                s.GetRequiredService<ModalNavigationStore>()
            ));

            services.AddTransient<ViewModelValidation>();
            services.AddTransient(s => new DashBoardViewModel
            (
                s.GetRequiredService<NavigationStore>(),
                s.GetRequiredService<ModalNavigationStore>()
            ));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            GlobalConfig.InitiallizeConnection(DatabaseType.TextFile, InitializeConfiguration());

            _serviceProvider.GetRequiredService<NavigationStore>().CurrentViewModel = _serviceProvider.GetRequiredService<DashBoardViewModel>();

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
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
