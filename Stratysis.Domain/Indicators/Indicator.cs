using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Domain.Indicators
{
    public abstract class Indicator<TValue>: IIndicator
    {
        public virtual void Calculate(Slice slice)
        {
            Values = new IndicatorSlice<TValue>(Values);
        }

        public bool IsWarmedUp { get; protected set; }

        public IndicatorSlice<TValue> Values { get; protected set; }

        public IndicatorSecuritySlice<TValue> this[string security] => Values[security];

        public void SetValue(string security, TValue value)
        {
            Values.Values[security] = value;
        }
    }
}
