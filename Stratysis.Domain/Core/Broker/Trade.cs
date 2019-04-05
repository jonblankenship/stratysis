using System;

namespace Stratysis.Domain.Core.Broker
{
    public class Trade
    {
        private readonly Order _order;
        private readonly FillDetails _fillDetails;

        public Trade(Order order, FillDetails fillDetails)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _fillDetails = fillDetails ?? throw new ArgumentNullException(nameof(fillDetails));
        }

        public Order Order => _order;

        public decimal FillPrice => _fillDetails.Price;
    }
}
