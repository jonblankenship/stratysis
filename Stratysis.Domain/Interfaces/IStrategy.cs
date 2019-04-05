using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Core;
using System;

namespace Stratysis.Domain.Interfaces
{
    public interface IStrategy
    {
        BacktestRun Initialize(IBroker broker, BacktestParameters parameters);

        void OnDataEvent(Slice slice);

        event EventHandler<Progress> ProgressReported;
    }
}
