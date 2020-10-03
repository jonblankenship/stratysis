using System;
using System.Collections.ObjectModel;
using System.Linq;
using FancyCandles;
using GalaSoft.MvvmLight;
using Stratysis.Domain.Interfaces;
using Stratysis.Wpf.Models;

namespace Stratysis.Wpf.ViewModels
{
    public class ChartsViewModel : ViewModelBase
    {
        private readonly IApplicationState _applicationState;

        public ChartsViewModel(IApplicationState applicationState)
        {
            _applicationState = applicationState;
            _applicationState.NewLastBacktestRun += ApplicationState_NewLastBacktestRun;
        }

        private void Progress_ProgressChanged(object sender, EventArgs e)
        {
            if (_applicationState.LastBacktestRun.Progress.IsComplete)
            {
                var candles = new ObservableCollection<ICandle>();
                foreach (var s in _applicationState.LastBacktestRun.Data)
                {
                    candles.Add(new Candle(s, s.Securities.First()));
                }
                Candles = candles;
                
                _applicationState.LastBacktestRun.Progress.ProgressChanged -= Progress_ProgressChanged;
            }
        }

        private void ApplicationState_NewLastBacktestRun(object sender, EventArgs e)
        {
            _applicationState.LastBacktestRun.Progress.ProgressChanged += Progress_ProgressChanged;
        }

        private ObservableCollection<ICandle> _candles;
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
