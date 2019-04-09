using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Stratysis.Engine
{
    /// <summary>
    /// Service to backtest trading strategies.  Main entry point for the backtesting engine.
    /// </summary>
    public class StrategyRunner: IStrategyRunner
    {
        private readonly IUniverseFactory _universeFactory;
        private readonly IDataManager _dataManager;
        private readonly IBroker _broker;

        public StrategyRunner(
            IUniverseFactory universeFactory, 
            IDataManager dataManager,
            IBroker broker)
        {
            _universeFactory = universeFactory ?? throw new ArgumentNullException(nameof(universeFactory));
            _dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
            _broker = broker ?? throw new ArgumentNullException(nameof(broker));
        }

        /// <summary>
        /// Runs a backtest of the given <see cref="strategy"/> using the <see cref="parameters"/> provided
        /// </summary>
        /// <param name="strategy">The <see cref="IStrategy"/> to test</param>
        /// <param name="parameters">The <see cref="BacktestParameters"/> to use for the test</param>
        /// <returns></returns>
        public async Task<BacktestRun> RunAsync(IStrategy strategy, BacktestParameters parameters)
        {
            var universe = _universeFactory.CreateUniverse(parameters.UniverseSelectionParameters);

            _broker.Reset(parameters.StartingCash);

            var backtestRun = strategy.Initialize(_broker, parameters);
            
            _dataManager.OnNewSlice += (sender, slice) => strategy.OnDataEvent(slice);

            await _dataManager.RequestDataAsync(parameters, universe);

            return backtestRun;
        }
    }
}
