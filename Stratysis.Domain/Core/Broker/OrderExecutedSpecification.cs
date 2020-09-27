using System;

namespace Stratysis.Domain.Core.Broker
{
    /// <summary>
    /// Specification to test whether an instance of an <see cref="Order"/> would have been filled as of a
    /// given <see cref="Slice"/> instance
    /// </summary>
    public class OrderExecutedSpecification
    {
        private readonly decimal _defaultCommission;
        private readonly Slice _slice;

        public OrderExecutedSpecification(decimal defaultCommission, Slice slice)
        {
            if (defaultCommission < 0) throw new ArgumentOutOfRangeException(nameof(defaultCommission));
            _defaultCommission = defaultCommission;
            _slice = slice ?? throw new ArgumentNullException(nameof(slice));
        }

        /// <summary>
        /// Evaluates whether <see cref="order"/> would have been filled as of the <see cref="Slice"/> instance
        /// used to instantiate this specification
        /// </summary>
        /// <param name="order">The <see cref="Order"/> instance to test</param>
        /// <returns>A tuple with a bool IsExecuted flag indicating whether the <see cref="order"/> would have
        /// been executed, and a FillDetails object with the fill details if IsExecuted == true</returns>
        public (bool IsExecuted, FillDetails FillDetails) IsSatisfiedBy(Order order)
        {
            var currentBar = _slice[order.Security].Bar;

            // Market orders
            if (order.Type == OrderTypes.Market)
            {
                return (true, new FillDetails(_slice.DateTime, currentBar.Open, _defaultCommission, order.Quantity));
            }

            // Non-market buy orders
            if (order.Action == OrderAction.Buy)
            {
                if (order.Type == OrderTypes.Limit)
                {
                    if (order.LimitPrice >= currentBar.Low)
                        return (true, new FillDetails(_slice.DateTime, Math.Min(currentBar.Open, order.LimitPrice), _defaultCommission, order.Quantity));
                }
                else if (order.Type == OrderTypes.Stop)
                {
                    if (order.StopPrice <= currentBar.High)
                        return (true, new FillDetails(_slice.DateTime, Math.Max(currentBar.Open, order.StopPrice), _defaultCommission, order.Quantity)); 
                }
            }

            // Non-market sell orders
            if (order.Action == OrderAction.Sell)
            {
                if (order.Type == OrderTypes.Limit)
                {
                    if (order.LimitPrice <= currentBar.High)
                        return (true, new FillDetails(_slice.DateTime, Math.Max(currentBar.Open, order.LimitPrice), _defaultCommission, order.Quantity));
                }
                else if (order.Type == OrderTypes.Stop)
                {
                    if (order.StopPrice >= currentBar.Low)
                        return (true, new FillDetails(_slice.DateTime, Math.Min(currentBar.Open, order.StopPrice), _defaultCommission, order.Quantity));
                }
            }

            return (false, null);
        }
    }
}
