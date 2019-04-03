using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Interfaces;
using Stratysis.Domain.Results;
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

        public async Task<BacktestResults> RunAsync(IStrategy strategy, BacktestParameters parameters)
        {
            var universe = _universeFactory.CreateUniverse(parameters.UniverseSelectionParameters);

            await _dataManager.RequestDataAsync(parameters, universe);

            return await Task.FromResult(new BacktestResults());
        }
    }
}
