using System;

namespace Stratysis.Domain.Backtesting
{
    public class Parameters
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public UniverseSelectionParameters UniverseSelectionParameters { get; set; }

        public int WarmupPeriod { get; set; } = 0;
    }
}
