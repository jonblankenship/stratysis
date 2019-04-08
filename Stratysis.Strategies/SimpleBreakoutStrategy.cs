using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Core;
using Stratysis.Domain.Core.Broker;
using Stratysis.Domain.Strategies;
using System;
using System.Diagnostics;
using Stratysis.Indicators;

namespace Stratysis.Strategies
{
    public class SimpleBreakoutStrategy: Strategy
    {
        private readonly int _entryBreakoutPeriod;
        private readonly int _exitBreakoutPeriod;
        private High _highLong;
        private High _highShort;
        private Low _lowLong;
        private Low _lowShort;

        public SimpleBreakoutStrategy(int entryBreakoutPeriod, int exitBreakoutPeriod)
        {
            if (entryBreakoutPeriod <= 0) throw new ArgumentOutOfRangeException(nameof(entryBreakoutPeriod));
            if (exitBreakoutPeriod <= 0) throw new ArgumentOutOfRangeException(nameof(exitBreakoutPeriod));
            _entryBreakoutPeriod = entryBreakoutPeriod;
            _exitBreakoutPeriod = exitBreakoutPeriod;
        }

        public override void Initialize(BacktestParameters parameters)
        {
            _highLong = new High(_entryBreakoutPeriod);
            _highShort = new High(_exitBreakoutPeriod);
            _lowLong = new Low(_entryBreakoutPeriod);
            _lowShort = new Low(_exitBreakoutPeriod);

            RegisterIndicator(_highLong);
            RegisterIndicator(_highShort);
            RegisterIndicator(_lowLong);
            RegisterIndicator(_lowShort);
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
                        if (slice[security].High > _highLong[security][-1])
                        {
                            BuyAtMarket(security, 100);
                            Debug.WriteLine($"{slice.DateTime} Entry: Buy at {slice[security][0].Close}");
                        }
                        else
                        {
                            if (slice[security].Low < _lowLong[security][-1])
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
                            if (slice[security].Low < _lowShort[security][-1])
                            {
                                SellAtMarket(security, 100);
                            }
                        }
                        else
                        {
                            if (slice[security].High < _highShort[security][-1])
                            {
                                BuyAtMarket(security, 100);
                            }
                        }
                    }
                }
            }
        }
    }
}
