using System;

namespace Stratysis.Domain.Core.Broker
{
    /// <summary>
    /// Domain class representing a trade
    /// </summary>
    public class Trade
    {
        private readonly Order _order;
        private readonly FillDetails _fillDetails;

        public Trade(Order order, FillDetails fillDetails, bool isExitTrade = false)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _fillDetails = fillDetails ?? throw new ArgumentNullException(nameof(fillDetails));
            IsExitTrade = isExitTrade;
        }

        /// <summary>
        /// The <see cref="Order"/> placed to initiate this <see cref="Trade"/>
        /// </summary>
        public Order Order => _order;

        /// <summary>
        /// The fill date/time for the <see cref="Trade"/>
        /// </summary>
        public DateTime FillDateTime => _fillDetails.DateTime;

        /// <summary>
        /// The fill price for the <see cref="Trade"/>
        /// </summary>
        public decimal FillPrice => _fillDetails.Price;

        /// <summary>
        /// The trade commission
        /// </summary>
        public decimal Commission => _fillDetails.Commission;

        /// <summary>
        /// The fill quantity for the <see cref="Trade"/>
        /// </summary>
        public int FillQuantity => _fillDetails.Quantity;

        /// <summary>
        /// Indicates whether this <see cref="Trade"/> reduced the size of a position, either partially or completely
        /// </summary>
        public bool IsExitTrade { get; }

        /// <summary>
        /// The realized gain/loss for this <see cref="Trade"/>, including commission
        /// </summary>
        public decimal RealizedGainLoss { get; set; }

        /// <summary>
        /// The realized gain/loss for this <see cref="Trade"/>, in points, excluding commission
        /// </summary>
        public decimal RealizedGainLossPoints { get; set; }
    }
}
