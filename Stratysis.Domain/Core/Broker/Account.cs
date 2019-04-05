using System.Collections.Generic;
using System.Linq;
using Stratysis.Domain.Backtesting;

namespace Stratysis.Domain.Core.Broker
{
    public class Account
    {
        private readonly List<Order> _orders = new List<Order>();

        private readonly List<Position> _positions = new List<Position>();

        public Account(decimal startingCash)
        {
            AccountBalance = startingCash;
        }

        public decimal AccountBalance { get; private set; }

        public IEnumerable<Order> Orders => _orders;

        public IEnumerable<Order> OpenOrders => _orders.Where(o => o.Status == OrderStatus.Open);

        public IEnumerable<Position> Positions => _positions;

        public IEnumerable<Position> OpenPositions => _positions.Where(p => p.Status == PositionStatus.Open);
        
        public void OpenOrder(Order order)
        {
            _orders.Add(order);
        }

        public void EvaluateOrders(decimal defaultCommission, Slice slice)
        {
            var spec = new OrderExecutedSpecification(defaultCommission, slice);
            foreach (var order in OpenOrders)
            {
                var specResult = spec.IsSatisfiedBy(order);
                if (specResult.IsExecuted)
                {
                    FillOrder(order, specResult.FillDetails);
                }
            }
        }

        public bool HasOpenPosition(string security) => OpenPositions.Any(p => p.Security == security);

        private void FillOrder(Order order, FillDetails fillDetails)
        {
            var openPosition = OpenPositions.FirstOrDefault(p => p.Security == order.Security);
            if (openPosition != null)
            {
                openPosition.FillOrder(order, fillDetails);
            }
            else
            {
                var newPosition = Position.InitiatePosition(order, fillDetails);
                _positions.Add(newPosition);
            }
        }
    }
}
