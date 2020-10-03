using Stratysis.Domain.Backtesting;

namespace Stratysis.Wpf.ViewModels
{
    public class BacktestParametersViewModel
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

        public BacktestParameters BacktestParameters => _parameters;
    }
}