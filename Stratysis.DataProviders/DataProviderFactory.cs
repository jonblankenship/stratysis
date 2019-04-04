using System;
using Stratysis.DataProviders.Quandl;
using Stratysis.DataProviders.Quandl.Clients;
using Stratysis.Domain.DataProviders;
using Stratysis.Domain.Interfaces;

namespace Stratysis.DataProviders
{
    public class DataProviderFactory: IDataProviderFactory
    {
        private readonly QuandlWebClient _quandlWebClient;
        private readonly QuandlFileClient _quandlFileClient;

        public DataProviderFactory(QuandlWebClient quandlWebClient, QuandlFileClient quandlFileClient)
        {
            _quandlWebClient = quandlWebClient ?? throw new ArgumentNullException(nameof(quandlWebClient));
            _quandlFileClient = quandlFileClient ?? throw new ArgumentNullException(nameof(quandlFileClient));
        }

        public IDataProvider CreateDataProvider(string symbol, DataProviderTypes type)
        {
            switch (type)
            {
                case DataProviderTypes.QuandlWeb:
                    return new QuandlDataProvider(symbol, _quandlWebClient);
                case DataProviderTypes.QuandlFile:
                    return new QuandlDataProvider(symbol, _quandlFileClient);
            }

            throw new NotImplementedException();
        }
    }
}
