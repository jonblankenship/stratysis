using System;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Strategies
{
    public class SimpleBreakoutStrategyParameters : IStrategyParameters
    {
        public int EntryBreakoutPeriod { get; set; } = 20;

        public int ExitBreakoutPeriod { get; set; } = 10;

        public int SmaPeriod { get; set; } = 20;

        public void Validate()
        {
            if (EntryBreakoutPeriod <= 0) throw new ArgumentOutOfRangeException(nameof(EntryBreakoutPeriod));
            if (ExitBreakoutPeriod <= 0) throw new ArgumentOutOfRangeException(nameof(ExitBreakoutPeriod));
            if (SmaPeriod <= 0) throw new ArgumentOutOfRangeException(nameof(SmaPeriod));
        }
    }
}
