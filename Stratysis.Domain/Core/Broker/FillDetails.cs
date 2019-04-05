using System;

namespace Stratysis.Domain.Core.Broker
{
    public class FillDetails
    {
        public FillDetails(DateTime dateTime, decimal price, decimal commission, int quantity)
        {
            DateTime = dateTime;
            Price = price;
            Commission = commission;
            Quantity = quantity;
        }

        public DateTime DateTime { get; }

        public decimal Price { get; }

        public decimal Commission { get; }

        public int Quantity { get; set; }
    }
}
