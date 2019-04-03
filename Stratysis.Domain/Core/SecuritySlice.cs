using System;
using System.Collections.Generic;
using System.Text;
using Stratysis.Domain.Indicators;

namespace Stratysis.Domain.Core
{
    public class SecuritySlice
    {
        private readonly Slice _parentSlice;
        private readonly string _symbol;
        private readonly Bar _bar;

        public SecuritySlice(Slice parentSlice, string symbol, Bar bar)
        {
            _parentSlice = parentSlice ?? throw new ArgumentNullException(nameof(parentSlice));
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            _bar = bar ?? throw new ArgumentNullException(nameof(bar));
        }

        public DateTime DateTime => _parentSlice.DateTime;

        public decimal High => _bar.High;

        public SecuritySlice this[int i]
        {
            get
            {
                if (i > 0)
                    throw new ArgumentOutOfRangeException(nameof(i), "Index must be less than or equal to zero.");

                if (i == 0)
                    return this;

                return _parentSlice[i][_symbol];
            }
        }
    }
}
