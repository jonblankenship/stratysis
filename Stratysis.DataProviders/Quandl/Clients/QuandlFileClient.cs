using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Stratysis.DataProviders.Quandl.Model;
using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;
using Stratysis.Domain.Settings;

namespace Stratysis.DataProviders.Quandl.Clients
{
    public class QuandlFileClient: IDataProviderClient
    {
        private readonly IDataProviderSettings _dataProviderSettings;

        public QuandlFileClient(IDataProviderSettings dataProviderSettings)
        {
            _dataProviderSettings = dataProviderSettings ?? throw new ArgumentNullException(nameof(dataProviderSettings));
        }

        public async Task<IEnumerable<Slice>> GetHistoricalDataAsync(string symbol, DateTime startDateTime, DateTime endDateTime)
        {
            var filePath = Path.Combine("../../../../", _dataProviderSettings.QuandlFolderPath, $"{ symbol}.csv");

            var contents = await File.ReadAllTextAsync(filePath);

            var quandlDataWrapper = QuandlDataWrapper.FromCsv(contents);

            return quandlDataWrapper.ToSlices(symbol);
        }
    }
}
