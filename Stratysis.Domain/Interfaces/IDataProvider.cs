using System;
using System.Threading.Tasks;
using Stratysis.Domain.Core;

namespace Stratysis.Domain.Interfaces
{
    public interface IDataProvider
    {
        Task RequestDataAsync(DateTime startDateTime, DateTime endDateTime);

        event EventHandler<Slice> OnDataReceived;
    }
}
