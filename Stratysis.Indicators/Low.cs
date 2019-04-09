using System;
using Stratysis.Domain.Core;
using Stratysis.Domain.Indicators;

namespace Stratysis.Indicators
{
    /// <summary>
    /// Low indicator
    /// </summary>
    /// <remarks>Calculates the low for the included securities for the given time period</remarks>
    public class Low: SingleValueIndicator<decimal>
    {
        private readonly int _period;

        /// <summary>
        /// Instantiates an instance of the Low indicator
        /// </summary>
        /// <param name="period">The number of slices to consider when calculating the low</param>
        public Low(int period)
        {
            if (period <= 0) throw new ArgumentOutOfRangeException(nameof(period));
            _period = period;
        }

        /// <summary>
        /// Calculates the indicator values for all securities in the <see cref="slice"/>
        /// </summary>
        /// <param name="slice">The current <see cref="Slice"/></param>
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
            try
            {
                if (!IsWarmedUp) return 0;

                var rangeLow = slice[0].Low;
                for (int i = period; i > 0; i--)
                {
                    if (slice[0 - i] is null)
                        continue;

                    var sliceLow = slice[0 - i].Low;
                    if (sliceLow < rangeLow)
                    {
                        rangeLow = sliceLow;
                    }
                }

                return rangeLow;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
