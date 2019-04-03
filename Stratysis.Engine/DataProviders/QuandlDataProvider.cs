using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stratysis.Engine.DataProviders
{
    public class QuandlDataProvider: IDataProvider
    {
        private readonly string _symbol;
        private readonly IWebDataProviderClient _client;

        public QuandlDataProvider(string symbol, IWebDataProviderClient client)
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
