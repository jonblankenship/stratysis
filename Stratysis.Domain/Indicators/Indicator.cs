using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Domain.Indicators
{
    /// <summary>
    /// Abstract class representing an indicator
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class Indicator<TValue>: IIndicator
    {
        /// <summary>
        /// Calculates the indicator values for all securities in the <see cref="slice"/>
        /// </summary>
        /// <param name="slice">The current <see cref="Slice"/></param>
        public virtual void Calculate(Slice slice)
        {
            Values = new IndicatorSlice<TValue>(Values);
        }

        /// <summary>
        /// Indicates whether the indicator instance has seen enough data to calculate its value(s)
        /// </summary>
        public bool IsWarmedUp { get; protected set; }

        /// <summary>
        /// The current value(s) of this indicator instance, represented as an <see cref="IndicatorSlice{TValue}"/>
        /// </summary>
        public IndicatorSlice<TValue> Values { get; protected set; }

        /// <summary>
        /// The current value(s) of this indicator instance for the given <see cref="security"/>
        /// </summary>
        /// <param name="security"></param>
        /// <returns></returns>
        public IndicatorSecuritySlice<TValue> this[string security] => Values[security];

        /// <summary>
        /// Sets the value of this indicator for the given <see cref="security"/>
        /// </summary>
        /// <param name="security"></param>
        /// <param name="value"></param>
        public void SetValue(string security, TValue value)
        {
            Values.Values[security] = value;
        }
    }
}
