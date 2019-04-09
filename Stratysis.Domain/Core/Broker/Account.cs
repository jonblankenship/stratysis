using System.Collections.Generic;
using System.Linq;

namespace Stratysis.Domain.Core.Broker
{
    /// <summary>
    /// Domain class representing an account with a broker
    /// </summary>
    public class Account
    {
        private readonly decimal _startingCash;

        private readonly List<Order> _orders = new List<Order>();

        private readonly List<Position> _positions = new List<Position>();

        /// <summary>
        /// Instantiates an <see cref="Account"/> instance with the <see cref="startingCash"/> balance
        /// </summary>
        /// <param name="startingCash">The starting cash amount with which to instantiate the <see cref="Account"/></param>
        public Account(decimal startingCash)
        {
            _startingCash = startingCash;
        }

        /// <summary>
        /// The account balance
        /// </summary>
        public decimal AccountBalance => _startingCash + Positions.Sum(p => p.RealizedGainLoss);

        /// <summary>
        /// The collection of open <see cref="Order"/>s for the account
        /// </summary>
        public IEnumerable<Order> OpenOrders => _orders.Where(o => o.Status == OrderStatus.Open);

        /// <summary>
        /// The collection of <see cref="Position"/>s held in the account, both open and closed
        /// </summary>
        public IEnumerable<Position> Positions => _positions;

        /// <summary>
        /// The collection of currently open <see cref="Positions"/> held in the account
        /// </summary>
        public IEnumerable<Position> OpenPositions => _positions.Where(p => p.Status == PositionStatus.Open);
        
        /// <summary>
        /// Opens an <see cref="Order"/> for the account
        /// </summary>
        /// <param name="order"></param>
        public void OpenOrder(Order order)
        {
            _orders.Add(order);
        }

        /// <summary>
        /// Evaluates the open <see cref="Order"/>s for the account as of the given <see cref="slice"/>
        /// </summary>
        /// <param name="defaultCommission"></param>
        /// <param name="slice"></param>
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

        /// <summary>
        /// Indicates whether this <see cref="Account"/> instance currently has any open <see cref="Position"/>s
        /// </summary>
        /// <param name="security"></param>
        /// <returns></returns>
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
