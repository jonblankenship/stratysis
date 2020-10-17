using System;
using System.Collections.Generic;
using System.Text;

namespace Stratysis.Domain.PositionSizing
{
    public enum PositionSizingMethods
    {
        FixedUnits,
        FixedDollars,
        TradePercentageOfAccountBalance,
        RiskPercentageOfAccountBalance
    }
}
