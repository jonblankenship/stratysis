using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Core;
using System;
using System.Threading.Tasks;

namespace Stratysis.Domain.Interfaces
{
    public interface IDataManager
    {
        Task RequestDataAsync(Parameters parameters, IUniverse universe);

        event EventHandler<Slice> OnNewSlice;
    }
}
