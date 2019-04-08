using System;
using Stratysis.Domain.Core;
using Stratysis.Domain.Indicators;

namespace Stratysis.Indicators
{
    public class High: SingleValueIndicator<decimal>
    {
        private readonly int _period;

        public High(int period)
        {
            if (period <= 0) throw new ArgumentOutOfRangeException(nameof(period));
            _period = period;
        }

        public override void Calculate(Slice slice)
        {
            base.Calculate(slice);

            IsWarmedUp = slice.SequenceNumber >= _period;

            foreach(var security in slice.Securities)
            {
                SetValue(security, GetPeriodHigh(slice[security], _period));
            }
        }

        private decimal GetPeriodHigh(SecuritySlice slice, int period)
        {
            if (!IsWarmedUp) return 0;

            var rangeHigh = slice[0].High;
            for (int i = period; i > 0; i--)
            {
                var sliceHigh = slice[0 - i]?.High ?? 0;
                if (sliceHigh > rangeHigh)
                {
                    rangeHigh = sliceHigh;
                }
            }

            return rangeHigh;
        }
    }
}
