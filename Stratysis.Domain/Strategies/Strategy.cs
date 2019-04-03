using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;
using System;

namespace Stratysis.Domain.Strategies
{
    public class Strategy: IStrategy
    {
        private Slice _lastSliceProcessed;

        public BacktestRun BacktestRun { get; private set; }

        public bool IsWarmedUp => BacktestRun.Parameters?.WarmupPeriod == 0 ||
                                  (_lastSliceProcessed?.SequenceNumber > BacktestRun.Parameters?.WarmupPeriod);

        public BacktestRun Initialize(Parameters parameters)
        {
            BacktestRun = new BacktestRun(parameters);
            return BacktestRun;
        }

        public void OnDataEvent(Slice slice)
        {
            PreProcessNewData(slice);

            ProcessNewData(slice);

            PostProcessNewData(slice);
        }

        private void PreProcessNewData(Slice slice)
        {
            // Calculate any indicator values for the slice, perform other pre-processing
        }

        protected virtual void ProcessNewData(Slice slice)
        {

        }

        private void PostProcessNewData(Slice slice)
        {
            _lastSliceProcessed = slice;

            ReportProgress(slice);
        }

        public event EventHandler<Progress> ProgressReported;

        private void ReportProgress(Slice slice)
        {
            BacktestRun.Progress.ProcessSlice(slice);
        }
    }
}
