using System.ComponentModel;
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

            ParametersViewModel.PropertyChanged += ParametersViewModelOnPropertyChanged;
        }

        public ParametersViewModel ParametersViewModel { get; }

        public ChartsViewModel ChartsViewModel { get; }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }
        
        private void ParametersViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ParametersViewModel.IsRunningBacktest))
            {
                IsBusy = ParametersViewModel.IsRunningBacktest;
            }
        }
    }
}
