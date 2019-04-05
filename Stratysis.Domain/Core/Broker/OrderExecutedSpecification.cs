using System;

namespace Stratysis.Domain.Core.Broker
{
    public class OrderExecutedSpecification
    {
        private readonly Slice _slice;

        public OrderExecutedSpecification(Slice slice)
        {
            _slice = slice ?? throw new ArgumentNullException(nameof(slice));
        }

        public (bool IsExecuted, FillDetails FillDetails) IsSatisfiedBy(Order order)
        {
            var currentBar = _slice[order.Security].Bar;

            // Market orders
            if (order.Type == OrderTypes.Market)
            {
                return (true, new FillDetails(_slice.DateTime, currentBar.Open, order.Quantity));
            }

            // Non-market buy orders
            if (order.Action == OrderAction.Buy)
            {
                if (order.Type == OrderTypes.Limit)
                {
                    if (order.LimitPrice >= currentBar.Low)
                        return (true, new FillDetails(_slice.DateTime, Math.Min(currentBar.Open, order.LimitPrice), order.Quantity));
                }
                else if (order.Type == OrderTypes.Stop)
                {
                    if (order.StopPrice <= currentBar.High)
                        return (true, new FillDetails(_slice.DateTime, Math.Max(currentBar.Open, order.StopPrice), order.Quantity)); 
                }
            }

            // Non-market sell orders
            if (order.Action == OrderAction.Sell)
            {
                if (order.Type == OrderTypes.Limit)
                {
                    if (order.LimitPrice <= currentBar.High)
                        return (true, new FillDetails(_slice.DateTime, Math.Max(currentBar.Open, order.LimitPrice), order.Quantity));
                }
                else if (order.Type == OrderTypes.Stop)
                {
                    if (order.StopPrice >= currentBar.Low)
                        return (true, new FillDetails(_slice.DateTime, Math.Min(currentBar.Open, order.StopPrice), order.Quantity));
                }
            }

            return (false, null);
        }
    }
}
