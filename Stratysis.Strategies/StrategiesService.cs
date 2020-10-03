using System;
using System.Collections.Generic;
using System.Linq;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Strategies
{
    public class StrategiesService : IStrategiesService 
    {
        private readonly Dictionary<Type, Type> _strategyParametersDictionary = new Dictionary<Type, Type>
        {
            { typeof(SimpleBreakoutStrategy), typeof(SimpleBreakoutStrategyParameters) }
        };

        public List<Type> GetStrategyTypes() => 
            (from s in _strategyParametersDictionary
             select s.Key).ToList();

        public IStrategy GetStrategy(Type type)
        {
            if (!_strategyParametersDictionary.ContainsKey(type))
                throw new ArgumentOutOfRangeException(nameof(type));

            return (IStrategy)Activator.CreateInstance(type);
        }

        public IStrategyParameters GetStrategyParameters<TStrategy>()
        {
            if (!_strategyParametersDictionary.ContainsKey(typeof(TStrategy)))
                throw new ArgumentOutOfRangeException(nameof(TStrategy));

            return (IStrategyParameters)Activator.CreateInstance(_strategyParametersDictionary[typeof(TStrategy)]);
        }
    }
}
