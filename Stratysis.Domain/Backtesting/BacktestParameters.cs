using System;
using Stratysis.Domain.Backtesting.Parameters;
using Stratysis.Domain.Core;
using Stratysis.Domain.DataProviders;

namespace Stratysis.Domain.Backtesting
{
    public class BacktestParameters
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public decimal StartingCash { get; set; }

        public decimal Commission { get; set; }

        public UniverseSelectionParameters UniverseSelectionParameters { get; set; }

        public DataProviderTypes DataProviderType { get; set; }
        
        public int WarmupPeriod { get; set; } = 0;

        public Granularities Granularity { get; set; } = Granularities.D;
    }
}
