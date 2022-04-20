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
using TrackerWPFUI.Services;
using TrackerWPFUI.ViewModels;
using TrackerWPFUI.ViewModels.Base;

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

            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton(s => new DashBoardViewModel(s.GetRequiredService<INotificationService>()));

            services.AddTransient<ViewModelValidation>();
            services.AddTransient(s => new NewTournamentViewModel(s.GetRequiredService<INotificationService>()));
            services.AddTransient(s => new NewTeamViewModel(s.GetRequiredService<INotificationService>()));
            services.AddTransient(s => new TournamentViewerViewModel(s.GetRequiredService<INotificationService>(),
                s.GetRequiredService<DashBoardViewModel>().SelectedTournament));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            GlobalConfig.InitiallizeConnection(DatabaseType.TextFile, InitializeConfiguration());

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            MainWindow.DataContext = new MainViewModel(_serviceProvider, _serviceProvider.GetRequiredService<INotificationService>());
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
