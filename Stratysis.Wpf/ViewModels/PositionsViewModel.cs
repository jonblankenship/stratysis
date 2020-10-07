using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Stratysis.Domain.Core.Broker;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Wpf.ViewModels
{
    public class PositionsViewModel : ViewModelBase
    {
        private readonly IApplicationState _applicationState;

        public PositionsViewModel(
            IApplicationState applicationState)
        {
            _applicationState = applicationState;
            _applicationState.NewLastBacktestRun += ApplicationState_NewLastBacktestRun;
        }

        private void Progress_ProgressChanged(object sender, EventArgs e)
        {
            if (_applicationState.LastBacktestRun.Progress.IsComplete)
            {
                Positions.Clear();
                foreach (var p in _applicationState.LastBacktestRun.Results.Positions)
                {
                    Positions.Add(p);
                }

                _applicationState.LastBacktestRun.Progress.ProgressChanged -= Progress_ProgressChanged;
            }
        }

        private void ApplicationState_NewLastBacktestRun(object sender, EventArgs e)
        {
            _applicationState.LastBacktestRun.Progress.ProgressChanged += Progress_ProgressChanged;
        }

        public ObservableCollection<Position> Positions { get; } = new ObservableCollection<Position>();
    }
}
