using System;
using System.Collections.Generic;
using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Domain.Backtesting
{
    public class BacktestRun
    {
        public BacktestRun(BacktestParameters parameters, IStrategyParameters strategyParameters)
        {
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            StrategyParameters = strategyParameters ?? throw new ArgumentNullException(nameof(parameters));
            Progress = new Progress(parameters);
            Results = new Results(parameters);
        }

        public BacktestParameters Parameters { get; }

        public IStrategyParameters StrategyParameters { get; }

        public Progress Progress { get; }

        public List<Slice> Data { get; } = new List<Slice>();

        public Results Results { get; }
    }
}
