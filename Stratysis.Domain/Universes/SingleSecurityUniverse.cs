using Stratysis.Domain.Backtesting;
using System;
using System.Collections.Generic;

namespace Stratysis.Domain.Universes
{
    public class SingleSecurityUniverse: Universe
    {
        private readonly SingleSecurityUniverseParameters _parameters;

        public SingleSecurityUniverse(SingleSecurityUniverseParameters parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public override IEnumerable<string> GetSecurities(DateTime asOfDateTime) => new List<string> {_parameters.Symbol};
    }
}
