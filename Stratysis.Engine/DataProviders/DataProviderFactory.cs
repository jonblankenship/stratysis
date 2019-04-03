using Stratysis.DataProviders.Quandl;
using Stratysis.Domain.DataProviders;
using Stratysis.Domain.Interfaces;
using System;

namespace Stratysis.Engine.DataProviders
{
    public class DataProviderFactory: IDataProviderFactory
    {
        private readonly QuandlClient _quandlClient;

        public DataProviderFactory(QuandlClient quandlClient)
        {
            _quandlClient = quandlClient ?? throw new ArgumentNullException(nameof(quandlClient));
        }

        public IDataProvider CreateDataProvider(string symbol, DataProviderTypes type)
        {
            if (type == DataProviderTypes.Quandl)
            {
                return new QuandlDataProvider(symbol, _quandlClient);
            }

            throw new NotImplementedException();
        }
    }
}
