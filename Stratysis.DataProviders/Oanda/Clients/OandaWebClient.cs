using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stratysis.DataProviders.Oanda.Model;
using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;
using Stratysis.Domain.Settings;

namespace Stratysis.DataProviders.Oanda.Clients
{
    public class OandaWebClient : IDataProviderClient
    {
        private readonly IDataProviderSettings _dataProviderSettings;
        private readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "https://api-fxpractice.oanda.com";
        private const int OandaMaxCandles = 5000;

        public OandaWebClient(IDataProviderSettings dataProviderSettings)
        {
            _dataProviderSettings = dataProviderSettings ?? throw new ArgumentNullException(nameof(dataProviderSettings));
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", dataProviderSettings.OandaApiKey);
        }

        public async Task<IEnumerable<Slice>> GetHistoricalDataAsync(string symbol, DateTime startDateTime, DateTime endDateTime, Granularities granularity)
        {
            TimeSpan requestInterval;

            switch (granularity)
            {
                case Granularities.M1:
                    requestInterval = TimeSpan.FromDays(OandaMaxCandles / (24 * 60));
                    break;
                case Granularities.M5:
                    requestInterval = TimeSpan.FromDays(OandaMaxCandles / (24 * 60 / 5));
                    break;
                case Granularities.D:
                    requestInterval = TimeSpan.FromDays(OandaMaxCandles);
                    break;
                default:
                    throw new ArgumentException($"Granularity {granularity} not supported.");
            }

            var results = new List<Slice>();
            var tempStartDateTime = startDateTime;
            var tempEndDateTime = startDateTime.Add(requestInterval);
            Slice prevSlice = null;
            do
            {
                if (tempEndDateTime > endDateTime)
                    tempEndDateTime = endDateTime;

                var uri = $"/v3/instruments/{symbol}/candles?granularity={granularity}&from={tempStartDateTime}&to={tempEndDateTime}";

                try
                {
                    var response = await _httpClient.GetAsync(uri);

                    response.EnsureSuccessStatusCode();

                    var jsonData = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<OandaDataWrapper>(jsonData);
                    var slices = data.ToSlices(symbol, startDateTime, endDateTime, prevSlice);
                    prevSlice = slices.Last();
                    results.AddRange(slices);

                    tempStartDateTime = tempEndDateTime.AddSeconds(1);
                    tempEndDateTime = tempEndDateTime.Add(requestInterval);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            } while (tempEndDateTime < endDateTime && tempEndDateTime <= DateTime.UtcNow);

            return results;
        }
    }
}
