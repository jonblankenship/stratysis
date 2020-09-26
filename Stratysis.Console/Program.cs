using Autofac;
using Microsoft.Extensions.Configuration;
using Stratysis.DataProviders;
using Stratysis.DataProviders.Quandl;
using Stratysis.DataProviders.Quandl.Clients;
using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Backtesting.Parameters;
using Stratysis.Domain.DataProviders;
using Stratysis.Domain.Interfaces;
using Stratysis.Domain.Settings;
using Stratysis.Domain.Universes;
using Stratysis.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Stratysis.DataProviders.Oanda;
using Stratysis.DataProviders.Oanda.Clients;
using Stratysis.Domain.Brokers;
using Stratysis.Domain.Core;
using Stratysis.Strategies;

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

            // Setup and run backtest
            //var backtestParameters = new BacktestParameters
            //{
            //    StartingCash = 10_000m,
            //    StartDateTime = new DateTime(2001, 1, 1),
            //    EndDateTime = new DateTime(2017, 12, 31),
            //    WarmupPeriod = 20,
            //    UniverseSelectionParameters = new MultipleSecurityUniverseParameters
            //    {
            //        Symbols = new List<string> { "MSFT", "V", "KO" }
            //    },
            //    DataProviderType = DataProviderTypes.QuandlWeb
            //};

            var backtestParameters = new BacktestParameters
            {
                StartingCash = 10_000m,
                StartDateTime = new DateTime(2020, 1, 1),
                EndDateTime = new DateTime(2020, 9, 30),
                WarmupPeriod = 20,
                UniverseSelectionParameters = new SingleSecurityUniverseParameters
                {
                    Symbol = "EUR_USD"
                },
                DataProviderType = DataProviderTypes.OandaWeb,
                Granularity = Granularities.M5
            };

            var strategy = new SimpleBreakoutStrategy(20, 10, 20);
            var runner = container.Resolve<IStrategyRunner>();

            var backtestRun = await runner.RunAsync(strategy, backtestParameters);
            
            System.Console.ReadKey();
        }

        static IContainer ConfigureIoC(IAppSettings appSettings)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<QuandlWebClient>();
            builder.RegisterType<QuandlFileClient>();
            builder.RegisterType<OandaWebClient>();
            builder.RegisterType<DataProviderFactory>().As<IDataProviderFactory>();
            builder.RegisterType<StrategyRunner>().As<IStrategyRunner>();
            builder.RegisterType<DataManager>().As<IDataManager>();
            builder.RegisterType<UniverseFactory>().As<IUniverseFactory>();
            builder.RegisterType<MockBroker>().As<IBroker>();
            builder.RegisterInstance(appSettings);
            builder.RegisterInstance(appSettings).As<IDataProviderSettings>();

            var container = builder.Build();
            return container;
        }
    }
}
