using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Core;
using System;
using System.Threading.Tasks;

namespace Stratysis.Domain.Interfaces
{
    /// <summary>
    /// Coordinates the creation of data providers, submission of data requests, and notification of new
    /// data for the securities included in a backtest
    /// </summary>
    public interface IDataManager
    {
        /// <summary>
        /// Requests data for all securities in the given <see cref="universe"/>
        /// </summary>
        /// <param name="parameters">The <see cref="BacktestParameters"/> for this run</param>
        /// <param name="universe">The <see cref="IUniverse"/> of securities for this run</param>
        /// <returns></returns>
        Task RequestDataAsync(BacktestParameters parameters, IUniverse universe);
        
        /// <summary>
        /// Notifies subscribers of a new <see cref="Slice"/> of data
        /// </summary>
        event EventHandler<Slice> OnNewSlice;
    }
}
