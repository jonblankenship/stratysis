using Stratysis.Domain.Backtesting.Parameters;

namespace Stratysis.Domain.Interfaces
{
    public interface IUniverseFactory
    {
        IUniverse CreateUniverse(UniverseSelectionParameters parameters);
    }
}
