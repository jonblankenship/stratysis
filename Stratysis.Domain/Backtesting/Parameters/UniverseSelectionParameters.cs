using Stratysis.Domain.Universes;

namespace Stratysis.Domain.Backtesting.Parameters
{
    public abstract class UniverseSelectionParameters
    {
        public abstract UniverseSelectionTypes Type { get; }
    }
}
