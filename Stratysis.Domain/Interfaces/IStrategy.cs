using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Core;
using System;
using Stratysis.Domain.PositionSizing;

namespace Stratysis.Domain.Interfaces
{
    public interface IStrategy
    {
        public string StrategyName { get; }

        BacktestRun Initialize(
            IBroker broker,
            IPositionSizer positionSizer, 
            BacktestParameters parameters, 
            IStrategyParameters strategyParameters);

        void OnDataEvent(object sender, Slice e);

        event EventHandler<Progress> ProgressReported;
    }
}
