using System;

namespace Stratysis.Domain.Backtesting
{
    public class BacktestParameters
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public UniverseSelectionParameters UniverseSelectionParameters { get; set; }
    }
}
