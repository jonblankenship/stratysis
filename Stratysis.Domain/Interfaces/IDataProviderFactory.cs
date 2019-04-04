using Stratysis.Domain.Backtesting.Parameters;
using Stratysis.Domain.DataProviders;

namespace Stratysis.Domain.Interfaces
{
    public interface IDataProviderFactory
    {
        IDataProvider CreateDataProvider(string symbol, DataProviderTypes type);
    }
}
