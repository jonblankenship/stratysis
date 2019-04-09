using Stratysis.Domain.Core;
using Stratysis.Domain.Indicators;
using System;

namespace Stratysis.Indicators
{
    /// <summary>
    /// Simple Moving Average indicator
    /// </summary>
    /// <remarks>Calculates the simple moving average for the included securities for the given time period</remarks>
    public class SimpleMovingAverage : SingleValueIndicator<decimal>
    {
        private readonly int _period;

        /// <summary>
        /// Instantiates an instance of the Simple Moving Average indicator
        /// </summary>
        /// <param name="period">The number of slices to consider when calculating the simple moving average</param>
        public SimpleMovingAverage(int period)
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
            
            foreach (var security in slice.Securities)
            {
                var sum = 0m;
                var hasEnoughData = true;
                for (int i = 0; i < _period; i++)
                {
                    var bar = slice[security][0 - i];
                    hasEnoughData = bar != null;
                    if (!hasEnoughData) break;

                    sum += slice[security][0 - i].Close;
                }

                if (hasEnoughData)
                    SetValue(security, sum / _period);
            }            
        }
    }
}
