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
        // Indicator parameters
        private readonly int _entryBreakoutPeriod;
        private readonly int _exitBreakoutPeriod;
        private readonly int _smaPeriod;

        // Indicators
        private High _highLong;
        private High _highShort;
        private Low _lowLong;
        private Low _lowShort;
        private SimpleMovingAverage _sma;

        public SimpleBreakoutStrategy(int entryBreakoutPeriod, int exitBreakoutPeriod, int smaPeriod)
        {
            if (entryBreakoutPeriod <= 0) throw new ArgumentOutOfRangeException(nameof(entryBreakoutPeriod));
            if (exitBreakoutPeriod <= 0) throw new ArgumentOutOfRangeException(nameof(exitBreakoutPeriod));
            if (smaPeriod <= 0) throw new ArgumentOutOfRangeException(nameof(smaPeriod));
            _entryBreakoutPeriod = entryBreakoutPeriod;
            _exitBreakoutPeriod = exitBreakoutPeriod;
            _smaPeriod = smaPeriod;
        }

        public override void Initialize(BacktestParameters parameters)
        {
            // Instantiate the indicators needed for this strategy
            _highLong = new High(_entryBreakoutPeriod);
            _highShort = new High(_exitBreakoutPeriod);
            _lowLong = new Low(_entryBreakoutPeriod);
            _lowShort = new Low(_exitBreakoutPeriod);
            _sma = new SimpleMovingAverage(_smaPeriod);

            // Register the indicators with the strategy
            RegisterIndicator(_highLong);
            RegisterIndicator(_highShort);
            RegisterIndicator(_lowLong);
            RegisterIndicator(_lowShort);
            RegisterIndicator(_sma);
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
                        // We don't have an open position for this security yet, so check entry conditions
                        EvaluateEntryConditions(security, slice);
                    }
                    else
                    {
                        // We already have an open position for this security, so check exit conditions
                        EvaluateExitConditions(openPosition, security, slice);
                    }
                }
            }
        }

        protected void EvaluateEntryConditions(string security, Slice slice)
        {
            if (AreIndicatorsWarmedUp(security, -3)) // Ensure all indicators are warmed up as of 3 periods ago
            {
                // Check for long entry criteria
                if (slice[security].High > _highLong[security][-1] && _sma[security][0] > _sma[security][-3])
                {
                    BuyAtMarket(security, 100);
                    Debug.WriteLine($"{slice.DateTime} Entry: Buy at {slice[security][0].Close}");
                }
                else
                {
                    // Check for short entry criteria
                    if (slice[security].Low < _lowLong[security][-1] && _sma[security][0] < _sma[security][-3])
                    {
                        SellAtMarket(security, 100);
                        Debug.WriteLine($"{slice.DateTime} Entry: Sell at {slice[security][0].Close}");
                    }
                }
            }
        }

        protected void EvaluateExitConditions(Position openPosition, string security, Slice slice)
        {
            if (openPosition.Direction == PositionDirection.Long)
            {
                // Check for long exit criteria
                if (slice[security].Low < _lowShort[security][-1])
                {
                    SellAtMarket(security, 100);
                }
            }
            else
            {
                // Check for short exit criteria
                if (slice[security].High < _highShort[security][-1])
                {
                    BuyAtMarket(security, 100);
                }
            }
        }
    }
}
