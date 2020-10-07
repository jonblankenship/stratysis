using System;
using System.Collections.ObjectModel;
using System.Linq;
using FancyCandles;
using GalaSoft.MvvmLight;
using Stratysis.Domain.Interfaces;
using Stratysis.Wpf.Models;

namespace Stratysis.Wpf.ViewModels
{
    public class CandlestickChartViewModel : ViewModelBase
    {
        private readonly IApplicationState _applicationState;

        public CandlestickChartViewModel(IApplicationState applicationState)
        {
            _applicationState = applicationState;
            _applicationState.NewLastBacktestRun += ApplicationState_NewLastBacktestRun;
        }

        private void Progress_ProgressChanged(object sender, EventArgs e)
        {
            if (_applicationState.LastBacktestRun.Progress.IsComplete)
            {
                Candles.Clear();
                foreach (var s in _applicationState.LastBacktestRun.Data)
                {
                    Candles.Add(new Candle(s, s.Securities.First()));
                }
                
                _applicationState.LastBacktestRun.Progress.ProgressChanged -= Progress_ProgressChanged;
            }
        }

        private void ApplicationState_NewLastBacktestRun(object sender, EventArgs e)
        {
            _applicationState.LastBacktestRun.Progress.ProgressChanged += Progress_ProgressChanged;
        }

        private ObservableCollection<ICandle> _candles = new ObservableCollection<ICandle> { new Candle(new DateTime(2020, 1, 1), 0, 0, 0, 0, 0) };
        public ObservableCollection<ICandle> Candles
        {
            get => _candles;
            set
            {
                _candles = value;
                RaisePropertyChanged(nameof(Candles));
            }
        }
    }
}
