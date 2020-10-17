using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using Stratysis.Domain.PositionSizing;

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
        private readonly IPositionSizer _positionSizer;
        private readonly IApplicationState _applicationState;

        public StrategyRunner(
            IUniverseFactory universeFactory, 
            IDataManager dataManager,
            IBroker broker,
            IPositionSizer positionSizer,
            IApplicationState applicationState)
        {
            _universeFactory = universeFactory ?? throw new ArgumentNullException(nameof(universeFactory));
            _dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
            _broker = broker ?? throw new ArgumentNullException(nameof(broker));
            _positionSizer = positionSizer ?? throw new ArgumentNullException(nameof(positionSizer));
            _applicationState = applicationState;
        }

        /// <summary>
        /// Runs a backtest of the given <see cref="strategy"/> using the <see cref="parameters"/> provided
        /// </summary>
        /// <param name="strategy">The <see cref="IStrategy"/> to test</param>
        /// <param name="parameters">The <see cref="BacktestParameters"/> to use for the test</param>
        /// <param name="strategyParameters">The strategy-specific <see cref="IStrategyParameters"/> to use for the test</param>
        /// <returns></returns>
        public async Task<BacktestRun> RunAsync(
            IStrategy strategy, 
            BacktestParameters parameters,
            IStrategyParameters strategyParameters)
        {
            var universe = _universeFactory.CreateUniverse(parameters.UniverseSelectionParameters);

            _broker.Reset(parameters.StartingCash);

            var backtestRun = strategy.Initialize(
                _broker, 
                _positionSizer,
                parameters, 
                strategyParameters);

            _applicationState.LastBacktestRun = backtestRun;

            _dataManager.OnNewSlice += strategy.OnDataEvent;

            await _dataManager.RequestDataAsync(parameters, universe);

            _dataManager.OnNewSlice -= strategy.OnDataEvent;

            return backtestRun;
        }
    }
}
