using System;
using Stratysis.Domain.Backtesting.Parameters;
using Stratysis.Domain.Core;
using Stratysis.Domain.DataProviders;
using Stratysis.Domain.PositionSizing;

namespace Stratysis.Domain.Backtesting
{
    public class BacktestParameters
    {
        public DateTime StartDateTime { get; set; } = new DateTime(2019, 1, 1);

        public DateTime EndDateTime { get; set; } = new DateTime(2020, 6, 30);

        public decimal StartingCash { get; set; } = 10_000m;

        public decimal Commission { get; set; } = 0;

        public UniverseSelectionParameters UniverseSelectionParameters { get; set; } = new SingleSecurityUniverseParameters { Symbol = "EUR_USD" };

        public PositionSizingParameters PositionSizingParameters { get; set; } = new PositionSizingParameters { Method = PositionSizingMethods.FixedUnits, Units = 10000 };

        public DataProviderTypes DataProviderType { get; set; } = DataProviderTypes.OandaWeb;
        
        public int WarmupPeriod { get; set; } = 20;

        public Granularities Granularity { get; set; } = Granularities.D;
    }
}
