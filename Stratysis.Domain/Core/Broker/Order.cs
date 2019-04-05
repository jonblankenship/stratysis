namespace Stratysis.Domain.Core.Broker
{
    public class Order
    {
        public string Security { get; set; }

        public OrderAction Action { get; set; }

        public OrderStatus Status { get; set; }

        public OrderTypes Type { get; set; }

        public int Quantity { get; set; }

        public decimal StopPrice { get; set; }

        public decimal LimitPrice { get; set; }
    }
}
