using System.Collections.Generic;
using Stratysis.Domain.Universes;

namespace Stratysis.Domain.Backtesting.Parameters
{
    public class MultipleSecurityUniverseParameters: UniverseSelectionParameters
    {
        public override UniverseSelectionTypes Type => UniverseSelectionTypes.MultipleSecurities;

        public IEnumerable<string> Symbols { get; set; }
    }
}
