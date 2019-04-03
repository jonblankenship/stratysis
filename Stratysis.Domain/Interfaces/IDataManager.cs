using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Universes;
using System.Threading.Tasks;

namespace Stratysis.Domain.Interfaces
{
    public interface IDataManager
    {
        Task RequestDataAsync(BacktestParameters parameters, IUniverse universe);
    }
}
