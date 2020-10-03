﻿using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Core;
using System;

namespace Stratysis.Domain.Interfaces
{
    public interface IStrategy
    {
        public string StrategyName { get; }

        BacktestRun Initialize(IBroker broker, BacktestParameters parameters, IStrategyParameters strategyParameters);

        void OnDataEvent(object sender, Slice e);

        event EventHandler<Progress> ProgressReported;
    }
}
