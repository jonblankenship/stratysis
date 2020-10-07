using System;
using GalaSoft.MvvmLight;
using Stratysis.Domain.Backtesting;

namespace Stratysis.Wpf.ViewModels
{
    public class BacktestParametersViewModel : ViewModelBase
    {
        private readonly BacktestParameters _parameters;

        public BacktestParametersViewModel()
        {
            _parameters = new BacktestParameters();
        }

        public decimal StartingCash
        {
            get => _parameters.StartingCash;
            set => _parameters.StartingCash = value;
        }

        public DateTime StartDateTime
        {
            get => _parameters.StartDateTime;
            set => _parameters.StartDateTime = value;
        }

        public DateTime EndDateTime
        {
            get => _parameters.EndDateTime;
            set => _parameters.EndDateTime = value;
        }

        public int WarmupPeriod
        {
            get => _parameters.WarmupPeriod;
            set => _parameters.WarmupPeriod = value;
        }

        public BacktestParameters BacktestParameters => _parameters;
    }
}