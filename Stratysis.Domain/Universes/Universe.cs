using Stratysis.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Stratysis.Domain.Universes
{
    public abstract class Universe: IUniverse
    {
        public abstract IEnumerable<string> GetSecurities(DateTime asOfDateTime);
    }
}
