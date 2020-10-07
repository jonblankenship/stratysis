using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using GalaSoft.MvvmLight;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Wpf.ViewModels
{
    public class BacktestResultsViewModel : ViewModelBase
    {
        private readonly IApplicationState _applicationState;

        public BacktestResultsViewModel(
            PositionsViewModel tradedViewModel,
            IApplicationState applicationState)
        {
            _applicationState = applicationState;
            _applicationState.NewLastBacktestRun += ApplicationState_NewLastBacktestRun;
        }

        public int? TotalClosedTrades => _applicationState.LastBacktestRun?.Results?.TotalClosedTrades;

        public int? RemainingOpenPositions => _applicationState.LastBacktestRun?.Results?.RemainingOpenPositions;

        public int? Wins => _applicationState.LastBacktestRun?.Results?.Wins;

        public int? Losses => _applicationState.LastBacktestRun?.Results?.Losses;

        public decimal? WinPercentage => _applicationState.LastBacktestRun?.Results?.WinPercentage;

        public decimal? Expectancy => _applicationState.LastBacktestRun?.Results?.Expectancy;

        public decimal? AverageWin => _applicationState.LastBacktestRun?.Results?.AverageWin;

        public decimal? AverageLoss => _applicationState.LastBacktestRun?.Results?.AverageLoss;

        public decimal? TotalRealizedGainLoss => _applicationState.LastBacktestRun?.Results?.TotalRealizedGainLoss;

        public decimal? TotalRealizedGainLossPoints => _applicationState.LastBacktestRun?.Results?.TotalRealizedGainLossPoints;

        public decimal? TotalUnrealizedGainLoss => _applicationState.LastBacktestRun?.Results?.TotalUnrealizedGainLoss;

        public decimal? GainLossPercentage => _applicationState.LastBacktestRun?.Results?.GainLossPercentage;

        public decimal? StartingAccountBalance => _applicationState.LastBacktestRun?.Results?.StartingAccountBalance;

        public decimal? FinalAccountBalance => _applicationState.LastBacktestRun?.Results?.FinalAccountBalance;

        public ObservableCollection<KeyValuePair<DateTime, decimal>> AccountBalanceSeries { get; set; } = new ObservableCollection<KeyValuePair<DateTime, decimal>>();

        private void Progress_ProgressChanged(object sender, EventArgs e)
        {
            if (_applicationState.LastBacktestRun.Progress.IsComplete)
            {
                AccountBalanceSeries.Clear();

                foreach (var b in _applicationState.LastBacktestRun.Results.AccountBalanceSeries)
                {
                    AccountBalanceSeries.Add(b);
                }

                RaisePropertyChanged(null);
            }
        }

        private void ApplicationState_NewLastBacktestRun(object sender, EventArgs e)
        {
            _applicationState.LastBacktestRun.Progress.ProgressChanged += Progress_ProgressChanged;
        }
    }
}
