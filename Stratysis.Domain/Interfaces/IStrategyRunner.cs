using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Results;
using System.Threading.Tasks;

namespace Stratysis.Domain.Interfaces
{
    public interface IStrategyRunner
    {
        Task<BacktestResults> RunAsync(IStrategy strategy, BacktestParameters parameters);
    }
}
