using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Stratysis.Domain.DataProviders
{
    public class DataManager: IDataManager
    {
        private readonly IDataProviderFactory _dataProviderFactory;

        public DataManager(IDataProviderFactory dataProviderFactory)
        { 
            _dataProviderFactory = dataProviderFactory ?? throw new ArgumentNullException(nameof(dataProviderFactory));
        }

        public async Task RequestDataAsync(BacktestParameters parameters, IUniverse universe)
        {
            var dataProviders = new List<IDataProvider>();

            // For now we'll just get all securities as of start date.  In the future
            // we'll handle dynamic universes where the securities in the universe can 
            // change during the course of a backtest.
            foreach (var security in universe.GetSecurities(parameters.StartDateTime))
            {
                var dataProvider = _dataProviderFactory.CreateDataProvider(security, DataProviderTypes.Quandl);
                dataProviders.Add(dataProvider);
            }

            foreach (var provider in dataProviders)
            {
                provider.OnDataReceived += ProviderOnOnDataReceived;
            }

            foreach (var provider in dataProviders)
            {
                await provider.RequestDataAsync(parameters.StartDateTime, parameters.EndDateTime);
            }
        }

        private void ProviderOnOnDataReceived(object sender, Slice e)
        {
            Debug.WriteLine("ProviderOnDataReceived");
        }
    }
}
