using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Stratysis.Domain.Core;

namespace Stratysis.DataProviders.Quandl.Model
{
    public class QuandlDataWrapper
    {
        [JsonProperty(PropertyName = "dataset_data")]
        public DatasetData DatasetData { get; set; }

        public IEnumerable<Slice> ToSlices(string symbol)
        {
            foreach (var dataPoint in DatasetData.Data)
            {
                var slice = new Slice
                {
                    DateTime = DateTime.Parse(GetValue<string>(DatasetData, dataPoint, "Date")),
                    Bars = new Dictionary<string, Bar>
                    {
                        {
                            symbol,
                            new Bar
                            {
                                Open = Convert.ToDecimal(GetValue<double>(DatasetData, dataPoint, "Open")),
                                High = Convert.ToDecimal(GetValue<double>(DatasetData, dataPoint, "High")),
                                Low = Convert.ToDecimal(GetValue<double>(DatasetData, dataPoint, "Low")),
                                Close = Convert.ToDecimal(GetValue<double>(DatasetData, dataPoint, "Close"))
                            }
                        }
                    }
                };

                yield return slice;
            }
        }

        private static T GetValue<T>(DatasetData dataset, IReadOnlyList<object> dataPoint, string name)
        {
            var index = dataset.ColumnNames.FindIndex(x => x == name);
            return (T) dataPoint[index];
        }
    }
}
