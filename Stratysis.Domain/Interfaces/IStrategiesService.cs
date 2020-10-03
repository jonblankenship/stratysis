using System;
using System.Collections.Generic;

namespace Stratysis.Domain.Interfaces
{
    public interface IStrategiesService
    {
        List<Type> GetStrategyTypes();

        IStrategy GetStrategy(Type type);

        IStrategyParameters GetStrategyParameters<TStrategy>();
    }
}
