using Stratysis.Domain.Universes;

namespace Stratysis.Domain.Backtesting
{
    public class SingleSecurityUniverseParameters: UniverseSelectionParameters
    {
        public override UniverseSelectionTypes Type => UniverseSelectionTypes.SingleSecurity;

        public string Symbol { get; set; }
    }
}
