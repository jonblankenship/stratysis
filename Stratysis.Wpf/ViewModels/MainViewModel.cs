using GalaSoft.MvvmLight;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IStrategyRunner _strategyRunner;

        public MainViewModel(IStrategyRunner strategyRunner)
        {
            _strategyRunner = strategyRunner;
        }
    }
}
