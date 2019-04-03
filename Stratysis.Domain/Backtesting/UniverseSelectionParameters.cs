using Stratysis.Domain.Universes;

namespace Stratysis.Domain.Backtesting
{
    public abstract class UniverseSelectionParameters
    {
        public abstract UniverseSelectionTypes Type { get; }
    }
}
