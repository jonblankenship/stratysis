using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Stratysis.Domain.Core;

namespace Stratysis.DataProviders.Quandl.Model
{
    public class QuandlDataWrapper
    {
        [JsonProperty(PropertyName = "dataset_data")]
        public DatasetData DatasetData { get; set; } = new DatasetData();

        public static QuandlDataWrapper FromCsv(string csvContents)
        {
            var quandlDataWrapper = new QuandlDataWrapper();

            var lines = csvContents.Split("\n");

            quandlDataWrapper.DatasetData.ColumnNames = lines.First().Split(',').ToList();

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',').ToList();
                quandlDataWrapper.DatasetData.Data.Add(values.Cast<object>().ToList());
            }

            return quandlDataWrapper;
        }

        public IEnumerable<Slice> ToSlices(string symbol, DateTime startDateTime, DateTime endDateTime)
        {
            Slice prevSlice = null;
            foreach (var dataPoint in DatasetData.Data)
            {
                if (dataPoint.Count > 1)
                {
                    var dateTime = DateTime.Parse(GetValue(DatasetData, dataPoint, "Date"));
                    if (dateTime >= startDateTime && dateTime <= endDateTime)
                    {
                        var slice = new Slice(prevSlice)
                        {
                            DateTime = dateTime,
                            Bars = new Dictionary<string, Bar>
                            {
                                {
                                    symbol,
                                    new Bar
                                    {
                                        Open = Convert.ToDecimal(GetValue(DatasetData, dataPoint, "Open")),
                                        High = Convert.ToDecimal(GetValue(DatasetData, dataPoint, "High")),
                                        Low = Convert.ToDecimal(GetValue(DatasetData, dataPoint, "Low")),
                                        Close = Convert.ToDecimal(GetValue(DatasetData, dataPoint, "Close"))
                                    }
                                }
                            }
                        };

                        yield return slice;
                        
                        prevSlice = slice;
                    }
                }
            }
        }

        private static string GetValue(DatasetData dataset, IReadOnlyList<object> dataPoint, string name)
        {
            var index = dataset.ColumnNames.FindIndex(x => x == name);
            return dataPoint[index].ToString();
        }
    }
}
