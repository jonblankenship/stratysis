using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
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
        private readonly IApplicationState _applicationState;
        private IStrategy _selectedStrategy;
        private readonly StrategyParametersViewModelFactory _strategyParametersViewModelFactory = new StrategyParametersViewModelFactory();

        public ParametersViewModel(
            IStrategiesService strategiesService,
            IStrategyRunner strategyRunner,
            IApplicationState applicationState)
        {
            _strategiesService = strategiesService;
            _strategyRunner = strategyRunner;
            _applicationState = applicationState;
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

        private bool _isRunningBacktest = false;
        public bool IsRunningBacktest
        {
            get => _isRunningBacktest;
            set
            {
                _isRunningBacktest = value;
                RaisePropertyChanged(nameof(IsRunningBacktest));
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
            IsRunningBacktest = true;

            await _strategyRunner.RunAsync(
                _selectedStrategy, 
                BacktestParametersViewModel.BacktestParameters, 
                StrategyParametersViewModel.StrategyParameters);

            if (_applicationState.LastBacktestRun.Progress.IsComplete)
            {
                IsRunningBacktest = false;
            }
            else
            {
                _applicationState.LastBacktestRun.Progress.ProgressChanged += Progress_ProgressChanged;
            }
        }

        private void Progress_ProgressChanged(object sender, EventArgs e)
        {
            if (((Progress)sender).IsComplete)
            {
                IsRunningBacktest = false;
                _applicationState.LastBacktestRun.Progress.ProgressChanged -= Progress_ProgressChanged;
            }
        }
    }
}
