using System;

namespace Stratysis.Domain.Core.Broker
{
    public class FillDetails
    {
        public FillDetails(DateTime dateTime, decimal price, int quantity)
        {
            DateTime = dateTime;
            Price = price;
            Quantity = quantity;
        }

        public DateTime DateTime { get; }

        public decimal Price { get; }

        public int Quantity { get; }
    }
}
