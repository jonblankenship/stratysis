using System;
using System.Collections.Generic;
using System.Text;

namespace Stratysis.Domain.Backtesting
{
    public class BacktestRun
    {
        public BacktestRun(Parameters parameters)
        {
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            Progress = new Progress(parameters);
            Results = new Results(parameters);
        }

        public Parameters Parameters { get; }

        public Progress Progress { get; }

        public Results Results { get; }
    }
}
