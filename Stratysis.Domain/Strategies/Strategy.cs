using Stratysis.Domain.Backtesting;
using Stratysis.Domain.Core;
using Stratysis.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Stratysis.Domain.Core.Broker;
using Stratysis.Domain.PositionSizing;

namespace Stratysis.Domain.Strategies
{
    public class Strategy: IStrategy
    {
        private IBroker _broker;
        protected IPositionSizer _positionSizer;
        private Slice _lastSliceProcessed;
        private readonly IList<IIndicator> _indicators = new List<IIndicator>();

        public string StrategyName => GetType().Name;

        public BacktestRun BacktestRun { get; private set; }

        public bool IsWarmedUp => BacktestRun.Parameters?.WarmupPeriod == 0 ||
                                  _lastSliceProcessed?.SequenceNumber > BacktestRun.Parameters?.WarmupPeriod;

        public BacktestRun Initialize(
            IBroker broker, 
            IPositionSizer positionSizer,
            BacktestParameters parameters, 
            IStrategyParameters strategyParameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            _broker = broker ?? throw new ArgumentNullException(nameof(broker));
            _positionSizer = positionSizer ?? throw new ArgumentNullException(nameof(positionSizer));
            _positionSizer.Initialize(parameters.PositionSizingParameters);
            BacktestRun = new BacktestRun(parameters, strategyParameters);

            _indicators.Clear();

            Initialize(parameters, strategyParameters);

            return BacktestRun;
        }

        public virtual void Initialize(BacktestParameters parameters, IStrategyParameters strategyParameters)
        { }

        public void OnDataEvent(object sender, Slice slice)
        {
            if (slice != null)
            {
                PreProcessNewData(slice);

                ProcessNewData(slice);

                PostProcessNewData(slice);
            }
            else
            {
                // End of data stream
                BacktestRun.Results.CalculateResults(_broker.DefaultAccount, _lastSliceProcessed);
                Debug.WriteLine(BacktestRun.Results);

                ReportProgress(null);
            }
        }

        public event EventHandler<Progress> ProgressReported;

        public bool AreIndicatorsWarmedUp(string security, int periodOffset) => _indicators.All(i => i.IsWarmedUp(security, periodOffset));

        public bool HasOpenPosition(string security) => _broker.HasOpenPosition(security);

        public Position GetOpenPosition(string security) => _broker.GetOpenPosition(security);

        public Account GetAccount() => _broker.DefaultAccount;
        
        public void BuyAtMarket(string security, int quantity)
        {
            var order = new Order
            {
                Action = OrderAction.Buy,
                Security = security,
                Quantity = quantity,
                Type = OrderTypes.Market
            };
            _broker.OpenOrder(order);
        }

        public void SellAtMarket(string security, int quantity)
        {
            var order = new Order
            {
                Action = OrderAction.Sell,
                Security = security,
                Quantity = quantity,
                Type = OrderTypes.Market
            };
            _broker.OpenOrder(order);
        }

        protected void RegisterIndicator(IIndicator indicator)
        {
            _indicators.Add(indicator);
        }

        private void PreProcessNewData(Slice slice)
        {
            BacktestRun.Data.Add(slice);

            // Calculate any indicator values for the slice, perform other pre-processing
            _broker.EvaluateOrders(slice);

            foreach (var indicator in _indicators)
            {
                indicator.Calculate(slice);
            }
        }

        protected virtual void ProcessNewData(Slice slice)
        {
        }

        private void PostProcessNewData(Slice slice)
        {
            _lastSliceProcessed = slice;

            ReportProgress(slice);
        }

        private void ReportProgress(Slice slice)
        {
            BacktestRun.Progress.ProcessSlice(slice);
        }
    }
}
