namespace Stratysis.Domain.Core.Broker
{
    /// <summary>
    /// Domain class representing an order submitted to a broker
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The security symbol for the <see cref="Order"/>
        /// </summary>
        public string Security { get; set; }

        /// <summary>
        /// The <see cref="OrderAction"/> for the <see cref="Order"/>
        /// </summary>
        public OrderAction Action { get; set; }

        /// <summary>
        /// The <see cref="OrderStatus"/> for the <see cref="Order"/>
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// The <see cref="OrderTypes"/> for the <see cref="Order"/>
        /// </summary>
        public OrderTypes Type { get; set; }

        /// <summary>
        /// The order quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The stop price for the order
        /// </summary>
        /// <remarks>Should be zero for market and limit orders; non-zero for stop orders</remarks>
        public decimal StopPrice { get; set; }

        /// <summary>
        /// The limit price for the order
        /// </summary>
        /// <remarks>Should be zero for market and stop orders; non-zero for limit orders</remarks>
        public decimal LimitPrice { get; set; }
    }
}
