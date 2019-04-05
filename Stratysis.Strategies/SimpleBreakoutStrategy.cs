using System;
using System.Diagnostics;
using Stratysis.Domain.Core;
using Stratysis.Domain.Core.Broker;
using Stratysis.Domain.Strategies;

namespace Stratysis.Strategies
{
    public class SimpleBreakoutStrategy: Strategy
    {
        private readonly int _entryBreakoutPeriod;
        private readonly int _exitBreakoutPeriod;

        public SimpleBreakoutStrategy(int entryBreakoutPeriod, int exitBreakoutPeriod)
        {
            if (entryBreakoutPeriod <= 0) throw new ArgumentOutOfRangeException(nameof(entryBreakoutPeriod));
            if (exitBreakoutPeriod <= 0) throw new ArgumentOutOfRangeException(nameof(exitBreakoutPeriod));
            _entryBreakoutPeriod = entryBreakoutPeriod;
            _exitBreakoutPeriod = exitBreakoutPeriod;
        }

        protected override void ProcessNewData(Slice slice)
        {
            if (IsWarmedUp)
            {
                foreach (var security in slice.Securities)
                {
                    var openPosition = GetOpenPosition(security);
                    if (openPosition == null)
                    {
                        var rangeHigh = GetPeriodHigh(slice[security][-1], _entryBreakoutPeriod);
                        if (slice[security].High > rangeHigh)
                        {
                            BuyAtMarket(security, 100);
                            Debug.WriteLine($"{slice.DateTime} Entry: Buy at {slice[security][0].Close}");
                        }
                        else
                        {
                            var rangeLow = GetPeriodLow(slice[security][-1], _entryBreakoutPeriod);
                            if (slice[security].Low < rangeLow)
                            {
                                SellAtMarket(security, 100);
                                Debug.WriteLine($"{slice.DateTime} Entry: Sell at {slice[security][0].Close}");
                            }
                        }
                    }
                    else
                    {
                        if (openPosition.Direction == PositionDirection.Long)
                        {
                            var rangeLow = GetPeriodLow(slice[security][-1], _exitBreakoutPeriod);
                            if (slice[security].Low < rangeLow)
                            {
                                SellAtMarket(security, 100);
                            }
                        }
                        else
                        {
                            var rangeHigh = GetPeriodHigh(slice[security][-1], _exitBreakoutPeriod);
                            if (slice[security].High < rangeHigh)
                            {
                                BuyAtMarket(security, 100);
                            }
                        }
                    }
                }
            }
        }

        private decimal GetPeriodHigh(SecuritySlice slice, int period)
        {
            var rangeHigh = slice[0].High;
            for (int i = period; i > 0; i--)
            {
                var sliceHigh = slice[0 - i].High;
                if (sliceHigh > rangeHigh)
                {
                    rangeHigh = sliceHigh;
                }
            }

            return rangeHigh;
        }

        private decimal GetPeriodLow(SecuritySlice slice, int period)
        {
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
