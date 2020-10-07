using System;
using GalaSoft.MvvmLight;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Wpf.ViewModels
{
    public class ResultsViewModel : ViewModelBase
    {
        private readonly IApplicationState _applicationState;

        public ResultsViewModel(
            BacktestResultsViewModel backtestResultsViewModel,
            PositionsViewModel tradedViewModel,
            CandlestickChartViewModel candlestickChartViewModel,
            IApplicationState applicationState)
        {
            BacktestResultsViewModel = backtestResultsViewModel;
            PositionsViewModel = tradedViewModel;
            CandlestickChartViewModel = candlestickChartViewModel;
            _applicationState = applicationState;
            _applicationState.NewLastBacktestRun += ApplicationState_NewLastBacktestRun;
        }

        public BacktestResultsViewModel BacktestResultsViewModel { get; }

        public PositionsViewModel PositionsViewModel { get; }

        public CandlestickChartViewModel CandlestickChartViewModel { get; }


        private void ApplicationState_NewLastBacktestRun(object sender, EventArgs e)
        {
        }
    }
}
