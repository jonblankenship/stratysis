using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratysis.Domain.Core
{
    public class Slice
    {
        private const int SliceCountToKeepInCache = 20;

        public Slice(Slice previousSlice)
        {
            if (previousSlice != null)
            {
                RecentSlices = new Queue<Slice>(previousSlice.RecentSlices);
                if (RecentSlices.Count == SliceCountToKeepInCache)
                    RecentSlices.Dequeue();
                RecentSlices.Enqueue(previousSlice);
                SequenceNumber = previousSlice.SequenceNumber + 1;
            }
            else
            {
                RecentSlices = new Queue<Slice>(SliceCountToKeepInCache);
                SequenceNumber = 0;
            }
        }

        public int SequenceNumber { get; }

        public DateTime DateTime { get; set; }
        
        public IDictionary<string, Bar> Bars { get; set; } = new Dictionary<string, Bar>();

        public IEnumerable<string> Securities => Bars.Keys;

        public Queue<Slice> RecentSlices { get; }

        public Slice this[int i]
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

        public SecuritySlice this[string symbol]
        {
            get
            {
                if (!Bars.ContainsKey(symbol))
                    return null;

                return new SecuritySlice(this, symbol, Bars[symbol]);
            }
        }

    }
}
