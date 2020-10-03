using System;
using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Interfaces;

namespace Stratysis.Engine
{
    public class ApplicationState : IApplicationState
    {
        private BacktestRun _lastBacktestRun;

        public BacktestRun LastBacktestRun
        {
            get => _lastBacktestRun;
            set
            {
                _lastBacktestRun = value;
                NewLastBacktestRun?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler NewLastBacktestRun;
    }
}
