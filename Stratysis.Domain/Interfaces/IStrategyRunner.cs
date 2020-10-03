using System.Threading.Tasks;
using Stratysis.Domain.Backtesting;

namespace Stratysis.Domain.Interfaces
{
    public interface IStrategyRunner
    {
        Task<BacktestRun> RunAsync(
            IStrategy strategy, 
            BacktestParameters parameters,
            IStrategyParameters strategyParameters);
    }
}
