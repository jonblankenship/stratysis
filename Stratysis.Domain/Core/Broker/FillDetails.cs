using System;

namespace Stratysis.Domain.Core.Broker
{
    /// <summary>
    /// Domain class representing the fill details for an <see cref="Order"/>
    /// </summary>
    public class FillDetails
    {
        public FillDetails(DateTime dateTime, decimal price, decimal commission, int quantity)
        {
            DateTime = dateTime;
            Price = price;
            Commission = commission;
            Quantity = quantity;
        }

        /// <summary>
        /// The fill date/time
        /// </summary>
        public DateTime DateTime { get; }

        /// <summary>
        /// The fill price
        /// </summary>
        public decimal Price { get; }

        /// <summary>
        /// The commission charged for the fill
        /// </summary>
        public decimal Commission { get; }

        /// <summary>
        /// The fill quantity
        /// </summary>
        public int Quantity { get; set; }
    }
}
