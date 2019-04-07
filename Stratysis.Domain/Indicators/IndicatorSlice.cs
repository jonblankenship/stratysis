using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratysis.Domain.Indicators
{
    public class IndicatorSlice<TValue>
    {
        private const int SliceCountToKeepInCache = 20;

        public IndicatorSlice (IndicatorSlice<TValue> previousSlice)
        {
            if (previousSlice != null)
            {
                RecentSlices = new Queue<IndicatorSlice<TValue>>(previousSlice.RecentSlices);
                if (RecentSlices.Count == SliceCountToKeepInCache)
                    RecentSlices.Dequeue();
                RecentSlices.Enqueue(previousSlice);
                SequenceNumber = previousSlice.SequenceNumber + 1;
            }
            else
            {
                RecentSlices = new Queue<IndicatorSlice<TValue>>(SliceCountToKeepInCache);
                SequenceNumber = 0;
            }
        }

        public int SequenceNumber { get; }

        public DateTime DateTime { get; set; }

        public IDictionary<string, TValue> Values { get; set; } = new Dictionary<string, TValue>();

        public IEnumerable<string> Securities => Values.Keys;

        public Queue<IndicatorSlice<TValue>> RecentSlices { get; }

        public IndicatorSlice<TValue> this[int i]
        {
            get
            {
                if (i > 0)
                    throw new ArgumentOutOfRangeException(nameof(i), "Index must be less than or equal to zero.");

                if (i == 0)
                    return this;

                var absIndex = Math.Abs(i);
                var tempSlice = this;
                while (absIndex > SliceCountToKeepInCache)
                {
                    tempSlice = RecentSlices.First();
                    absIndex = absIndex - SliceCountToKeepInCache;
                }
                return tempSlice.RecentSlices.Reverse().Skip(absIndex - 1).FirstOrDefault();
            }
        }

        public IndicatorSecuritySlice<TValue> this[string symbol]
        {
            get
            {
                if (!Values.ContainsKey(symbol))
                    return null;

                return new IndicatorSecuritySlice<TValue>(this, symbol, Values[symbol]);
            }
        }
    }
}
