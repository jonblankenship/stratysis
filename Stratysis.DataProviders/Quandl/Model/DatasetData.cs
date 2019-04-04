using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stratysis.DataProviders.Quandl.Model
{
    public class DatasetData
    {
        [JsonProperty(PropertyName = "limit")]
        public object Limit { get; set; }

        [JsonProperty(PropertyName = "transform")]
        public object Transform { get; set; }

        [JsonProperty(PropertyName = "column_index")]
        public object ColumnIndex { get; set; }

        [JsonProperty(PropertyName = "column_names")]
        public List<string> ColumnNames { get; set; } = new List<string>();

        [JsonProperty(PropertyName = "start_date")]
        public string StartDate { get; set; }

        [JsonProperty(PropertyName = "end_date")]
        public string EndDate { get; set; }

        [JsonProperty(PropertyName = "frequency")]
        public string Frequency { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<List<object>> Data { get; set; } = new List<List<object>>();

        [JsonProperty(PropertyName = "collapse")]
        public object Collapse { get; set; }

        [JsonProperty(PropertyName = "order")]
        public object Order { get; set; }
    }
}
