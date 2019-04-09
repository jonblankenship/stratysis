using Stratysis.Domain.Backtesting.Parameters;
using System;
using System.Collections.Generic;

namespace Stratysis.Domain.Universes
{
    public class MultipleSecurityUniverse: Universe
    {
        private readonly MultipleSecurityUniverseParameters _parameters;

        public MultipleSecurityUniverse(MultipleSecurityUniverseParameters parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public override IEnumerable<string> GetSecurities(DateTime asOfDateTime) => _parameters.Symbols;
    }
}
