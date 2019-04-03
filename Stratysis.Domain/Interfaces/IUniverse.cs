using System;
using System.Collections.Generic;

namespace Stratysis.Domain.Interfaces
{
    public interface IUniverse
    {
        IEnumerable<string> GetSecurities(DateTime asOfDateTime);
    }
}
