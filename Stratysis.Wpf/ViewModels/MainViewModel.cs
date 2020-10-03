using GalaSoft.MvvmLight;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IStrategyRunner _strategyRunner;

        public MainViewModel(
            IStrategyRunner strategyRunner,
            ParametersViewModel parametersViewModel,
            ChartsViewModel chartsViewModel)
        {
            _strategyRunner = strategyRunner;
            ParametersViewModel = parametersViewModel;
            ChartsViewModel = chartsViewModel;
        }

        public ParametersViewModel ParametersViewModel { get; }

        public ChartsViewModel ChartsViewModel { get; }
    }
}
