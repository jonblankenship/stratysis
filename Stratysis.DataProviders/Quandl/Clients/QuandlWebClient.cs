using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stratysis.DataProviders.Quandl.Model;
using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;
using Stratysis.Domain.Settings;

namespace Stratysis.DataProviders.Quandl.Clients
{
    public class QuandlWebClient: IDataProviderClient
    {
        private readonly IDataProviderSettings _dataProviderSettings;
        private readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "https://www.quandl.com";

        public QuandlWebClient(IDataProviderSettings dataProviderSettings)
        {
            _dataProviderSettings = dataProviderSettings ?? throw new ArgumentNullException(nameof(dataProviderSettings));
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<IEnumerable<Slice>> GetHistoricalDataAsync(string symbol, DateTime startDateTime, DateTime endDateTime)
        {
            var uri = $"/api/v3/datasets/EOD/{symbol}/data.json?api_key={_dataProviderSettings.QuandlApiKey}&start_date={startDateTime:yyyy-MM-dd}&end_date={endDateTime:yyyy-MM-dd}&order=asc";
            var response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var jsonData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<QuandlDataWrapper>(jsonData);

            return data.ToSlices(symbol, startDateTime, endDateTime);
        }
    }
}
