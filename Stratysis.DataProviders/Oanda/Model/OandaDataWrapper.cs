using System;
using System.Collections.Generic;
using System.Text;
using Stratysis.Domain.Core;

namespace Stratysis.DataProviders.Oanda.Model
{
    public class OandaDataWrapper
    {
        public string Instrument { get; set; }
        
        public string Granularity { get; set; }

        public List<OandaCandle> Candles { get; set; }
        
        public IEnumerable<Slice> ToSlices(string symbol, DateTime startDateTime, DateTime endDateTime, Slice prevSlice = null)
        {
            foreach (var candle in Candles)
            {
                var slice = new Slice(prevSlice)
                {
                    DateTime = candle.Time,
                    Bars = new Dictionary<string, Bar>
                    {
                        {
                            symbol,
                            new Bar
                            {
                                Open = candle.Mid.O,
                                High = candle.Mid.H,
                                Low = candle.Mid.L,
                                Close = candle.Mid.C
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
