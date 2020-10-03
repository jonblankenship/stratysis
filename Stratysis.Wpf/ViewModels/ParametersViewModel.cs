using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Interfaces;
using Stratysis.Wpf.Commands;
using Stratysis.Wpf.ViewModels.StrategyParameters;

namespace Stratysis.Wpf.ViewModels
{
    public class ParametersViewModel : ViewModelBase
    {
        private readonly IStrategiesService _strategiesService;
        private readonly IStrategyRunner _strategyRunner;
        private IStrategy _selectedStrategy;
        private readonly StrategyParametersViewModelFactory _strategyParametersViewModelFactory = new StrategyParametersViewModelFactory();

        public ParametersViewModel(
            IStrategiesService strategiesService,
            IStrategyRunner strategyRunner)
        {
            _strategiesService = strategiesService;
            _strategyRunner = strategyRunner;
            foreach (var strategyType in strategiesService.GetStrategyTypes())
            {
                StrategyTypes.Add(strategyType);
            }

            SelectedStrategyType = StrategyTypes.FirstOrDefault();

            BacktestParametersViewModel = new BacktestParametersViewModel();

            RunCommand = new AsyncRelayCommand(ExecuteRunCommandAsync, CanExecuteRunCommand);
        }

        public ObservableCollection<Type> StrategyTypes { get; } = new ObservableCollection<Type>();

        private Type _selectedStrategyType;
        public Type SelectedStrategyType
        {
            get => _selectedStrategyType;
            set
            {
                _selectedStrategyType = value;
                _selectedStrategy = _strategiesService.GetStrategy(_selectedStrategyType);
                StrategyParametersViewModel = _strategyParametersViewModelFactory.CreateViewModel(_selectedStrategy.GetType());
            }
        }

        public BacktestParametersViewModel BacktestParametersViewModel { get; set; }

        private IStrategyParametersViewModel _strategyParametersViewModel;
        public IStrategyParametersViewModel StrategyParametersViewModel
        {
            get => _strategyParametersViewModel;
            set
            {
                _strategyParametersViewModel = value;
                RaisePropertyChanged(nameof(StrategyParametersViewModel));
            }
        }

        public ICommand RunCommand { get; set; }
        
        private bool CanExecuteRunCommand(object arg)
        {
            if (_selectedStrategy == null) return false;

            if (StrategyParametersViewModel == null) return false;

            return true;
        }

        private async Task ExecuteRunCommandAsync(object arg)
        {
            var results = await _strategyRunner.RunAsync(
                _selectedStrategy, 
                BacktestParametersViewModel.BacktestParameters, 
                StrategyParametersViewModel.StrategyParameters);
        }
    }
}
