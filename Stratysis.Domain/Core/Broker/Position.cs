using System;
using System.Collections.Generic;

namespace Stratysis.Domain.Core.Broker
{
    public class Position
    {
        private readonly List<Trade> _trades = new List<Trade>();

        public Position(Order order, FillDetails fillDetails)
        {
            Security = order.Security;
            Status = PositionStatus.Open;
            Direction = order.Action == OrderAction.Buy ? PositionDirection.Long : PositionDirection.Short;
            PositionSize = fillDetails.Quantity;
            
            _trades.Add(new Trade(order, fillDetails));
        }

        public string Security { get; }

        public PositionStatus Status { get; private set; }

        public PositionDirection Direction { get; }

        public decimal RealizedGainLoss { get; private set; }

        public int PositionSize { get; private set; }

        public IEnumerable<Trade> Trades => _trades;

        public void FillOrder(Order order, FillDetails fillDetails)
        {
            _trades.Add(new Trade(order, fillDetails));

            if (Direction == PositionDirection.Long)
            {
                if (order.Action == OrderAction.Buy)
                {
                    // Adding to long position
                    PositionSize += fillDetails.Quantity;
                }
                else
                {
                    // Reducing or closing long position
                    if (fillDetails.Quantity > PositionSize)
                        throw new ApplicationException("Fill quantity exceeds position size.");

                    PositionSize -= fillDetails.Quantity;
                    if (PositionSize == 0)
                    {
                        Status = PositionStatus.Closed;
                    }

                    CalculateRealizedGainLoss();
                }
            }
        }

        public static Position InitiatePosition(Order order, FillDetails fillDetails)
        {
            return new Position(order, fillDetails);
        }

        public decimal GetUnrealizedGainLoss(Bar asOfBar)
        {
            throw new NotImplementedException();
        }

        private void CalculateRealizedGainLoss()
        {
            throw new NotImplementedException();
        }
    }
}
