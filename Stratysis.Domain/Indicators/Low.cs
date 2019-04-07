using Stratysis.Domain.Core;
using System;

namespace Stratysis.Domain.Indicators
{
    public class Low: SingleValueIndicator<decimal>
    {
        private readonly int _period;

        public Low(int period)
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
                SetValue(security, GetPeriodLow(slice[security], _period));
            }
        }

        private decimal GetPeriodLow(SecuritySlice slice, int period)
        {
            if (!IsWarmedUp) return 0;

            var rangeLow = slice[0].Low;
            for (int i = period; i > 0; i--)
            {
                var sliceLow = slice[0 - i].Low;
                if (sliceLow < rangeLow)
                {
                    rangeLow = sliceLow;
                }
            }

            return rangeLow;
        }
    }
}
