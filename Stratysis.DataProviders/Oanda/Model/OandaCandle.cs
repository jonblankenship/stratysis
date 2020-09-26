using System;
using System.Collections.Generic;
using System.Text;

namespace Stratysis.DataProviders.Oanda.Model
{
    public class OandaCandle
    {
        public bool Complete { get; set; }

        public int Volume { get; set; }

        public DateTime Time { get; set; }

        public OandaBar Bid { get; set; }

        public OandaBar Ask { get; set; }

        public OandaBar Mid { get; set; }
    }
}
