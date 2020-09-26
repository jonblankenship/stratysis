using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Stratysis.Domain.DataProviders
{
    /// <summary>
    /// Coordinates the creation of data providers, submission of data requests, and notification of new
    /// data for the securities included in a backtest
    /// </summary>
    public class DataManager: IDataManager
    {
        private readonly IDataProviderFactory _dataProviderFactory;

        public DataManager(IDataProviderFactory dataProviderFactory)
        { 
            _dataProviderFactory = dataProviderFactory ?? throw new ArgumentNullException(nameof(dataProviderFactory));
        }

        /// <summary>
        /// Requests data for all securities in the given <see cref="universe"/>
        /// </summary>
        /// <param name="parameters">The <see cref="BacktestParameters"/> for this run</param>
        /// <param name="universe">The <see cref="IUniverse"/> of securities for this run</param>
        /// <returns></returns>
        public async Task RequestDataAsync(BacktestParameters parameters, IUniverse universe)
        {
            var dataSet = new SliceSet();

            // For now we'll just get all securities as of start date.  In the future
            // we'll handle dynamic universes where the securities in the universe can 
            // change during the course of a backtest.
            foreach (var security in universe.GetSecurities(parameters.StartDateTime))
            {
                var dataProvider = _dataProviderFactory.CreateDataProvider(security, parameters.DataProviderType);
                var data = await dataProvider.RequestDataAsync(parameters.StartDateTime, parameters.EndDateTime, parameters.Granularity);
                dataSet.Merge(data);
            }

            foreach (var slice in dataSet)
            {
                OnNewSlice?.Invoke(this, slice.Value);                
            }

            // Invoke with null value to indicate end of stream
            OnNewSlice?.Invoke(this, null);
        }

        /// <summary>
        /// Notifies subscribers of a new <see cref="Slice"/> of data
        /// </summary>
        public event EventHandler<Slice> OnNewSlice;
    }
}
