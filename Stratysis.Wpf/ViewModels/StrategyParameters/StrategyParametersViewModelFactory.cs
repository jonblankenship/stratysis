using System;
using System.Collections.Generic;
using Stratysis.Strategies;

namespace Stratysis.Wpf.ViewModels.StrategyParameters
{
    public class StrategyParametersViewModelFactory
    {
        private readonly Dictionary<Type, Type> _strategyParametersViewModelMappings = new Dictionary<Type, Type>
        {
            { typeof(SimpleBreakoutStrategy), typeof(SimpleBreakoutStrategyParametersViewModel) }
        };

        public IStrategyParametersViewModel CreateViewModel(Type strategyType)
        {
            if (!_strategyParametersViewModelMappings.ContainsKey(strategyType))
                throw new ArgumentOutOfRangeException(nameof(strategyType));

            return (IStrategyParametersViewModel)Activator.CreateInstance(_strategyParametersViewModelMappings[strategyType]);
        }
    }
}
