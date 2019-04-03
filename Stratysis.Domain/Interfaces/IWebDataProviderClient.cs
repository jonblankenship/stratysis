using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stratysis.Domain.Core;

namespace Stratysis.Domain.Interfaces
{
    public interface IWebDataProviderClient
    {
        Task<IEnumerable<Slice>> GetHistoricalDataAsync(string symbol, DateTime startDateTime, DateTime endDateTime);
    }
}
