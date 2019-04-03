using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stratysis.DataProviders.Quandl;
using Stratysis.Domain.Backtesting;
using Stratysis.Domain.DataProviders;
using Stratysis.Domain.Interfaces;
using Stratysis.Domain.Settings;
using Stratysis.Domain.Strategies;
using Stratysis.Domain.Universes;
using Stratysis.Engine;
using Stratysis.Engine.DataProviders;

namespace Stratysis.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Set up settings
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();
            var appSettings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettings);

            // Configure IoC
            var container = ConfigureIoC(appSettings);

            
            var backtestParameters = new Parameters
            {
                StartDateTime = new DateTime(2015, 1, 1),
                EndDateTime = new DateTime(2017, 12, 31),
                WarmupPeriod = 20,
                UniverseSelectionParameters = new SingleSecurityUniverseParameters
                {
                    Symbol = "MSFT"
                }
            };

            var strategy = new SimpleBreakoutStrategy(20);
            
            var runner = container.Resolve<IStrategyRunner>();
            var backtestRun = await runner.RunAsync(strategy, backtestParameters);

            backtestRun.Progress.ProgressChanged += (sender, eventArgs) =>
            {
                System.Console.WriteLine($"Percent complete: {(sender as Progress).PercentComplete:P}");
            };

            System.Console.ReadKey();
        }

        static IContainer ConfigureIoC(IAppSettings appSettings)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<QuandlDataProvider>().As<IDataProvider>();
            builder.RegisterType<QuandlClient>();
            builder.RegisterType<DataProviderFactory>().As<IDataProviderFactory>();
            builder.RegisterType<StrategyRunner>().As<IStrategyRunner>();
            builder.RegisterType<DataManager>().As<IDataManager>();
            builder.RegisterType<UniverseFactory>().As<IUniverseFactory>();
            builder.RegisterInstance(appSettings);
            builder.RegisterInstance(appSettings).As<IDataProviderSettings>();

            var container = builder.Build();
            return container;
        }
    }
}
