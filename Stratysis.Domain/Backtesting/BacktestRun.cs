using System;

namespace Stratysis.Domain.Backtesting
{
    public class BacktestRun
    {
        public BacktestRun(BacktestParameters parameters)
        {
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            Progress = new Progress(parameters);
            Results = new Results(parameters);
        }

        public BacktestParameters Parameters { get; }

        public Progress Progress { get; }

        public Results Results { get; }
    }
}
