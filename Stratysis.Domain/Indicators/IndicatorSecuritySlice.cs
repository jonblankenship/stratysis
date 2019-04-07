using System;

namespace Stratysis.Domain.Indicators
{
    public class IndicatorSecuritySlice<TValue>
    {
        private readonly IndicatorSlice<TValue> _parentSlice;
        private readonly string _symbol;

        public IndicatorSecuritySlice(IndicatorSlice<TValue> parentSlice, string symbol, TValue value)
        {
            _parentSlice = parentSlice ?? throw new ArgumentNullException(nameof(parentSlice));
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            if (value == null) throw new ArgumentNullException(nameof(value));
            Value = value;
        }

        public DateTime DateTime => _parentSlice.DateTime;

        public TValue Value { get; }

        public TValue this[int i]
        {
            get
            {
                if (i > 0)
                    throw new ArgumentOutOfRangeException(nameof(i), "Index must be less than or equal to zero.");

                if (i == 0)
                    return this.Value;

                return _parentSlice[i][_symbol].Value;
            }
        }
    }
}
