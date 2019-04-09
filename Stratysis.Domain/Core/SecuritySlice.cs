using System;

namespace Stratysis.Domain.Core
{
    public class SecuritySlice
    {
        private readonly Slice _parentSlice;
        private readonly string _symbol;

        public SecuritySlice(Slice parentSlice, string symbol, Bar bar)
        {
            _parentSlice = parentSlice ?? throw new ArgumentNullException(nameof(parentSlice));
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Bar = bar ?? throw new ArgumentNullException(nameof(bar));
        }

        public DateTime DateTime => _parentSlice.DateTime;

        public Bar Bar { get; }

        public decimal Open => Bar.Open;

        public decimal High => Bar.High;

        public decimal Low => Bar.Low;

        public decimal Close => Bar.Close;

        public SecuritySlice this[int i]
        {
            get
            {
                if (i > 0)
                    throw new ArgumentOutOfRangeException(nameof(i), "Index must be less than or equal to zero.");

                if (i == 0)
                    return this;

                if (_parentSlice[i] != null)                    
                    return _parentSlice[i][_symbol];

                return null;
            }
        }
    }
}
