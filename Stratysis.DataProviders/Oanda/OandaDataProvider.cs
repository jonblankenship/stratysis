using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;

namespace Stratysis.DataProviders.Oanda
{
    public class OandaDataProvider : IDataProvider
    {
        private readonly string _symbol;
        private readonly IDataProviderClient _client;

        public OandaDataProvider(string symbol, IDataProviderClient client)
        {
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<Slice>> RequestDataAsync(DateTime startDateTime, DateTime endDateTime, Granularities granularity)
        {
            return await _client.GetHistoricalDataAsync(_symbol, startDateTime, endDateTime, granularity);
        }
    }
}
