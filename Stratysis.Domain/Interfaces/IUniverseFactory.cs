using Stratysis.Domain.Backtesting;

namespace Stratysis.Domain.Interfaces
{
    public interface IUniverseFactory
    {
        IUniverse CreateUniverse(UniverseSelectionParameters parameters);
    }
}
