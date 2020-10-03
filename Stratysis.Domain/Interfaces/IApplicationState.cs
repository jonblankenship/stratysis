using System;
using Stratysis.Domain.Backtesting;

namespace Stratysis.Domain.Interfaces
{
    public interface IApplicationState
    {
        BacktestRun LastBacktestRun { get; set; }
        
        event EventHandler NewLastBacktestRun;
    }
}
