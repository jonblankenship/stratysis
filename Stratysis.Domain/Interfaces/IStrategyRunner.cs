using Stratysis.Domain.Backtesting;

namespace Stratysis.Domain.Interfaces
{
    public interface IStrategyRunner
    {
        BacktestRun Run(IStrategy strategy, BacktestParameters parameters);
    }
}
