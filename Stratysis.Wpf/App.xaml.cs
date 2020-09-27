using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stratysis.DataProviders;
using Stratysis.DataProviders.Oanda.Clients;
using Stratysis.DataProviders.Quandl.Clients;
using Stratysis.Domain.Brokers;
using Stratysis.Domain.DataProviders;
using Stratysis.Domain.Interfaces;
using Stratysis.Domain.Settings;
using Stratysis.Domain.Universes;
using Stratysis.Engine;
using Stratysis.Wpf.ViewModels;

namespace Stratysis.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;
        private AppSettings _settings;

        public App()
        {
            _host = new HostBuilder()
                        .ConfigureAppConfiguration((context, configurationBuilder) =>
                        {
                            configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                            configurationBuilder.AddJsonFile("appsettings.json", optional: false);
                            configurationBuilder.AddUserSecrets<App>();
                            
                            IConfigurationRoot configuration = configurationBuilder.Build();
                            _settings = new AppSettings();
                            configuration.GetSection("AppSettings").Bind(_settings);
                        })
                        .ConfigureServices((context, services) =>
                        {
                            services.AddSingleton<IAppSettings>(_settings);
                            services.AddSingleton<IDataProviderSettings>(_settings);
                            services.AddSingleton<QuandlWebClient>();
                            services.AddSingleton<QuandlFileClient>();
                            services.AddSingleton<OandaWebClient>();
                            services.AddSingleton<IDataProviderFactory, DataProviderFactory>();
                            services.AddSingleton<IStrategyRunner, StrategyRunner>();
                            services.AddSingleton<IDataManager, DataManager>();
                            services.AddSingleton<IUniverseFactory, UniverseFactory>();
                            services.AddSingleton<IBroker, MockBroker>();

                            // Register ViewModels
                            services.AddSingleton<MainViewModel>();

                            // Register Views
                            services.AddSingleton<MainWindow>();
                        })
                        .ConfigureLogging(logging =>
                        {
                            logging.AddConsole();
                        })
                        .Build();
            
            ServiceProvider = _host.Services;
        }

        public static IServiceProvider ServiceProvider { get; private set; }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }
        }
    }
}
