using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using FancyCandles;
using GalaSoft.MvvmLight;
using Stratysis.Domain.Core;
using Stratysis.Wpf.Models;

namespace Stratysis.Wpf.ViewModels
{
    public class ChartsViewModel : ViewModelBase
    {
        public ChartsViewModel()
        {
            //Slice prevSlice = null;
            //var currSlice = new Slice(prevSlice)
            //{
            //    DateTime = new DateTime(2020, 1, 1),
            //    Bars = new Dictionary<string, Bar>
            //    {
            //        {"EUR_USD", new Bar {Open = 1.2003M, High = 1.2045M, Low = 1.1923M, Close = 1.2009M}}
            //    }
            //};

            //Candles.Add(new Candle(currSlice, "EUR_USD"));

            //for (var i = 0; i <= 100; i++)
            //{
            //    prevSlice = currSlice;
            //    var random = new Random();
            //    currSlice = new Slice(prevSlice)
            //    {
            //        DateTime = prevSlice.DateTime.AddDays(1),
            //        Bars = new Dictionary<string, Bar>
            //        {
            //            {"EUR_USD", new Bar
            //            {
            //                Open = prevSlice.Bars["EUR_USD"].Close,
            //                High = prevSlice.Bars["EUR_USD"].Close + (random.Next(5, 200) / 10000M),
            //                Low = prevSlice.Bars["EUR_USD"].Close - (random.Next(5, 200) / 10000M),
            //                Close = prevSlice.Bars["EUR_USD"].Close
            //            }}
            //        }
            //    };
            //    var range = (currSlice.Bars["EUR_USD"].High - currSlice.Bars["EUR_USD"].Low) * 10000;
            //    currSlice.Bars["EUR_USD"].Close = currSlice.Bars["EUR_USD"].Low + (random.Next(0, (int)range) / 10000M);

            //    Candles.Add(new Candle(currSlice, "EUR_USD"));
            //}
        }

        public ObservableCollection<ICandle> Candles { get; } = new ObservableCollection<ICandle>();
    }
}
