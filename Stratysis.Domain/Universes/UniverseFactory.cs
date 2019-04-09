using Stratysis.Domain.Backtesting.Parameters;
using Stratysis.Domain.Interfaces;
using System;

namespace Stratysis.Domain.Universes
{
    public class UniverseFactory: IUniverseFactory
    {
        public IUniverse CreateUniverse(UniverseSelectionParameters parameters)
        {
            switch (parameters.Type)
            {
                case UniverseSelectionTypes.SingleSecurity:
                    return new SingleSecurityUniverse(parameters as SingleSecurityUniverseParameters);
                case UniverseSelectionTypes.MultipleSecurities:
                    return new MultipleSecurityUniverse(parameters as MultipleSecurityUniverseParameters);
            }

            throw new NotImplementedException();
        }
    }
}
