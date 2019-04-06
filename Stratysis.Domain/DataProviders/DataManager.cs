using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;
using System;
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
            var dataSet = new SliceSet();

            // For now we'll just get all securities as of start date.  In the future
            // we'll handle dynamic universes where the securities in the universe can 
            // change during the course of a backtest.
            foreach (var security in universe.GetSecurities(parameters.StartDateTime))
            {
                var dataProvider = _dataProviderFactory.CreateDataProvider(security, parameters.DataProviderType);
                dataSet.Merge(await dataProvider.RequestDataAsync(parameters.StartDateTime, parameters.EndDateTime));
            }

            foreach (var slice in dataSet)
            {
                OnNewSlice?.Invoke(this, slice.Value);                
            }

            // Invoke with null value to indicate end of stream
            OnNewSlice?.Invoke(this, null);
        }

        public event EventHandler<Slice> OnNewSlice;
    }
}
