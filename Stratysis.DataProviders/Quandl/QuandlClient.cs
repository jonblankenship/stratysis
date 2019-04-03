using Newtonsoft.Json;
using Stratysis.Domain.Core;
using Stratysis.Domain.DataProviders;
using Stratysis.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Stratysis.DataProviders.Quandl.Model;
using Stratysis.Domain.Interfaces;

namespace Stratysis.DataProviders.Quandl
{
    public class QuandlClient: IWebDataProviderClient
    {
        private readonly IDataProviderSettings _dataProviderSettings;
        private readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "https://www.quandl.com";

        public QuandlClient(IDataProviderSettings dataProviderSettings)
        {
            _dataProviderSettings = dataProviderSettings ?? throw new ArgumentNullException(nameof(dataProviderSettings));
        }

        public async Task<IEnumerable<Slice>> GetHistoricalDataAsync(string symbol, DateTime startDateTime, DateTime endDateTime)
        {
            _httpClient.BaseAddress = new Uri(BaseUrl);

            var uri = $"/api/v3/datasets/EOD/{symbol}/data.json?api_key={_dataProviderSettings.QuandlApiKey}";
            var response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var jsonData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<QuandlDataWrapper>(jsonData);

            return data.ToSlices(symbol);
        }
    }
}
