using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Interfaces;
using System;

namespace Stratysis.Domain.Universes
{
    public class UniverseFactory: IUniverseFactory
    {
        public IUniverse CreateUniverse(UniverseSelectionParameters parameters)
        {
            if (parameters.Type == UniverseSelectionTypes.SingleSecurity)
                return new SingleSecurityUniverse(parameters as SingleSecurityUniverseParameters);

            throw new NotImplementedException();
        }
    }
}
