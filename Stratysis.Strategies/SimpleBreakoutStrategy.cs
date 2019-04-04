﻿using System;
using System.Diagnostics;
using Stratysis.Domain.Core;
using Stratysis.Domain.Strategies;

namespace Stratysis.Strategies
{
    public class SimpleBreakoutStrategy: Strategy
    {
        private readonly int _breakoutPeriod;

        public SimpleBreakoutStrategy(int breakoutPeriod)
        {
            if (breakoutPeriod <= 0) throw new ArgumentOutOfRangeException(nameof(breakoutPeriod));
            _breakoutPeriod = breakoutPeriod;
        }

        protected override void ProcessNewData(Slice slice)
        {
            if (IsWarmedUp)
            {
                foreach (var security in slice.Securities)
                {
                    var isNewHigh = true;
                    for (int i = _breakoutPeriod; i > 0; i--)
                    {
                        if (slice[security][0 - i].High > slice[security][0].High)
                        {
                            isNewHigh = false;
                            break;
                        }
                    }

                    if (isNewHigh)
                        Debug.WriteLine($"New High: {slice.DateTime} {security} {slice[security][0].High}");
                }
            }
        }
    }
}