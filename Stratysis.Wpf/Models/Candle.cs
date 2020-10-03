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
            O = (double)slice[security].Open;
            H = (double)slice[security].High;
            L = (double)slice[security].Low;
            C = (double)slice[security].Close;
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
