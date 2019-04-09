using System;
using Stratysis.Domain.Core;
using Stratysis.Domain.Indicators;

namespace Stratysis.Indicators
{
    /// <summary>
    /// High indicator
    /// </summary>
    /// <remarks>Calculates the high for the included securities for the given time period</remarks>
    public class High: SingleValueIndicator<decimal>
    {
        private readonly int _period;

        /// <summary>
        /// Instantiates an instance of the High indicator
        /// </summary>
        /// <param name="period">The number of slices to consider when calculating the high</param>
        public High(int period)
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
                SetValue(security, GetPeriodHigh(slice[security], _period));
            }
        }

        private decimal GetPeriodHigh(SecuritySlice slice, int period)
        {
            try
            {
                if (!IsWarmedUp) return 0;

                var rangeHigh = slice[0].High;
                for (int i = period; i > 0; i--)
                {
                    if (slice[0 - i] is null)
                        continue;

                    var sliceHigh = slice[0 - i]?.High ?? 0;
                    if (sliceHigh > rangeHigh)
                    {
                        rangeHigh = sliceHigh;
                    }
                }

                return rangeHigh;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
