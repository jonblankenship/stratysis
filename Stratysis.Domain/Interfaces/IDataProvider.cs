using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stratysis.Domain.Core;

namespace Stratysis.Domain.Interfaces
{
    public interface IDataProvider
    {
        Task<IEnumerable<Slice>> RequestDataAsync(DateTime startDateTime, DateTime endDateTime);
    }
}
