using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Stratysis.Engine
{
    public class StrategyRunner: IStrategyRunner
    {
        private readonly IUniverseFactory _universeFactory;
        private readonly IDataManager _dataManager;

        public StrategyRunner(
            IUniverseFactory universeFactory, 
            IDataManager dataManager)
        {
            _universeFactory = universeFactory ?? throw new ArgumentNullException(nameof(universeFactory));
            _dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
        }

        public async Task<BacktestRun> RunAsync(IStrategy strategy, Parameters parameters)
        {
            var universe = _universeFactory.CreateUniverse(parameters.UniverseSelectionParameters);

            var backtestRun = strategy.Initialize(parameters);
            
            _dataManager.OnNewSlice += (sender, slice) => strategy.OnDataEvent(slice);

            _dataManager.RequestDataAsync(parameters, universe);

            return backtestRun;
        }
    }
}
