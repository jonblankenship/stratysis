using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;

namespace Stratysis.DataProviders.Quandl
{
    public class QuandlDataProvider: IDataProvider
    {
        private readonly string _symbol;
        private readonly IDataProviderClient _client;

        public QuandlDataProvider(string symbol, IDataProviderClient client)
        {
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<Slice>> RequestDataAsync(DateTime startDateTime, DateTime endDateTime)
        {
            return await _client.GetHistoricalDataAsync(_symbol, startDateTime, endDateTime);
        }
    }
}
