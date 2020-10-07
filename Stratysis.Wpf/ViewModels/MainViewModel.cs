using System.ComponentModel;
using GalaSoft.MvvmLight;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(
            ParametersViewModel parametersViewModel,
            ResultsViewModel resultsViewModel)
        {
            ParametersViewModel = parametersViewModel;
            ResultsViewModel = resultsViewModel;;
            
            ParametersViewModel.PropertyChanged += ParametersViewModelOnPropertyChanged;
        }

        public ParametersViewModel ParametersViewModel { get; }

        public ResultsViewModel ResultsViewModel { get; }

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
