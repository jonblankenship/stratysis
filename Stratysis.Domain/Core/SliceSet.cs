using System;
using System.Collections.Generic;

namespace Stratysis.Domain.Core
{
    public class SliceSet: Dictionary<DateTime, Slice>
    {
        public void Merge(IEnumerable<Slice> setToMerge)
        {
            foreach (var slice in setToMerge)
            {
                if (ContainsKey(slice.DateTime))
                {
                    foreach (var bar in slice.Bars)
                    {
                        this[slice.DateTime].Bars[bar.Key] = slice.Bars[bar.Key];
                    }
                }
                else
                {
                    Add(slice.DateTime, slice);
                }
            }
        }
    }
}
