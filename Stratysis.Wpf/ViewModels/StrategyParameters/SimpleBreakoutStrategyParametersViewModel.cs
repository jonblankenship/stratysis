using Stratysis.Domain.Interfaces;
using Stratysis.Strategies;

namespace Stratysis.Wpf.ViewModels.StrategyParameters
{
    public class SimpleBreakoutStrategyParametersViewModel : IStrategyParametersViewModel
    {
        private readonly SimpleBreakoutStrategyParameters _parameters;

        public SimpleBreakoutStrategyParametersViewModel()
        {
            _parameters = new SimpleBreakoutStrategyParameters();
        }

        public int EntryBreakoutPeriod
        {
            get => _parameters.EntryBreakoutPeriod;
            set => _parameters.EntryBreakoutPeriod = value;
        }

        public int ExitBreakoutPeriod
        {
            get => _parameters.ExitBreakoutPeriod;
            set => _parameters.ExitBreakoutPeriod = value;
        }

        public int SmaPeriod
        {
            get => _parameters.SmaPeriod;
            set => _parameters.SmaPeriod = value;
        }

        public IStrategyParameters StrategyParameters => _parameters;
    }
}