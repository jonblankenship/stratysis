using Stratysis.Domain.Backtesting;
using System.Threading.Tasks;

namespace Stratysis.Domain.Interfaces
{
    public interface IStrategyRunner
    {
        Task<BacktestRun> RunAsync(IStrategy strategy, Parameters parameters);
    }
}
