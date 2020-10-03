using System;
using Stratysis.Domain.Core;

namespace Stratysis.Wpf.Models
{
    public class Candle : FancyCandles.ICandle
    {
        public Candle(DateTime t, double O, double H, double L, double C, long V)
        {
            this.t = t;
            this.O = O;
            this.H = H;
            this.L = L;
            this.C = C;
            this.V = V;
        }

        public Candle(Slice slice, string security)
        {
            t = slice.DateTime;
            O = Math.Round((double)slice[security].Open, 5);
            H = Math.Round((double)slice[security].High, 5);
            L = Math.Round((double)slice[security].Low, 5);
            C = Math.Round((double)slice[security].Close, 5);
            V = 0;
        }

        public DateTime t { get; set; }

        public double O { get; set; }

        public double H { get; set; }

        public double L { get; set; }

        public double C { get; set; }

        public long V { get; set; }
    }
}
