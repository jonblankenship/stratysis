using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            
            order.Status = OrderStatus.Executed;

            CalculateRealizedGainLoss();
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

            if ((Direction == PositionDirection.Long && order.Action == OrderAction.Buy) ||
                (Direction == PositionDirection.Short && order.Action == OrderAction.Sell))
            {
                // Adding to position
                PositionSize += fillDetails.Quantity;
            }
            else
            {
                // Reducing or closing position
                if (fillDetails.Quantity > PositionSize)
                    throw new ApplicationException("Fill quantity exceeds position size.");

                PositionSize -= fillDetails.Quantity;
                if (PositionSize == 0)
                {
                    Status = PositionStatus.Closed;
                }
            }

            order.Status = OrderStatus.Executed;

            CalculateRealizedGainLoss();

            if (Status == PositionStatus.Closed)
                Debug.WriteLine($"{fillDetails.DateTime} Position closed: {RealizedGainLoss}");
        }

        public static Position InitiatePosition(Order order, FillDetails fillDetails)
        {
            return new Position(order, fillDetails);
        }

        public decimal GetUnrealizedGainLoss(Bar asOfBar)
        {
            if (Direction == PositionDirection.Long) return CalculatedLongUnrealizedGainLoss(asOfBar);

            return CalculatedShortUnrealizedGainLoss(asOfBar);
        }

        private decimal CalculatedLongUnrealizedGainLoss(Bar asOfBar)
        {
            // Calculate unrealized gain/loss using FIFO method
            var fillsQueue = new Queue<FillDetails>();
            foreach (var trade in Trades.OrderBy(t => t.FillDateTime))
            {
                if (trade.Order.Action == OrderAction.Buy)
                {
                    fillsQueue.Enqueue(new FillDetails(trade.FillDateTime, trade.FillPrice, trade.Commission, trade.FillQuantity));
                }
                else
                {
                    var fillQuantity = trade.FillQuantity;
                    while (fillQuantity > 0)
                    {
                        while (fillQuantity >= fillsQueue.FirstOrDefault()?.Quantity)
                        {
                            fillQuantity -= fillsQueue.First().Quantity;
                            fillsQueue.Dequeue();
                        }

                        if (fillQuantity > 0 && fillQuantity < fillsQueue.FirstOrDefault()?.Quantity)
                        {
                            var fill = fillsQueue.First();
                            fill.Quantity -= fillQuantity;
                            fillQuantity = 0;
                        }
                    }
                }
            }

            return fillsQueue.Sum(f => (asOfBar.Close - f.Price) * f.Quantity);
        }

        private decimal CalculatedShortUnrealizedGainLoss(Bar asOfBar)
        {
            // Calculate unrealized gain/loss using FIFO method
            var fillsQueue = new Queue<FillDetails>();
            foreach (var trade in Trades.OrderBy(t => t.FillDateTime))
            {
                if (trade.Order.Action == OrderAction.Sell)
                {
                    fillsQueue.Enqueue(new FillDetails(trade.FillDateTime, trade.FillPrice, trade.Commission, trade.FillQuantity));
                }
                else
                {
                    var fillQuantity = trade.FillQuantity;
                    while (fillQuantity > 0)
                    {
                        while (fillQuantity >= fillsQueue.FirstOrDefault()?.Quantity)
                        {
                            fillQuantity -= fillsQueue.First().Quantity;
                            fillsQueue.Dequeue();
                        }

                        if (fillQuantity > 0 && fillQuantity < fillsQueue.FirstOrDefault()?.Quantity)
                        {
                            var fill = fillsQueue.First();
                            fill.Quantity -= fillQuantity;
                            fillQuantity = 0;
                        }
                    }
                }
            }

            return fillsQueue.Sum(f => (f.Price - asOfBar.Close) * f.Quantity);
        }

        private void CalculateRealizedGainLoss()
        {
            if (Direction == PositionDirection.Long) CalculateLongRealizedGainLoss();
            else CalculateShortRealizedGainLoss();
        }

        private void CalculateLongRealizedGainLoss()
        {
            // Calculate gain/loss using FIFO method
            var realizedGainLoss = 0m;
            var fillsQueue = new Queue<FillDetails>();
            foreach (var trade in Trades.OrderBy(t => t.FillDateTime))
            {
                realizedGainLoss -= trade.Commission;
                if (trade.Order.Action == OrderAction.Buy)
                {
                    fillsQueue.Enqueue(new FillDetails(trade.FillDateTime, trade.FillPrice, trade.Commission, trade.FillQuantity));
                }
                else
                {
                    var fillQuantity = trade.FillQuantity;
                    while (fillQuantity > 0)
                    {
                        while (fillQuantity >= fillsQueue.FirstOrDefault()?.Quantity)
                        {
                            realizedGainLoss += (trade.FillPrice - fillsQueue.First().Price) * fillsQueue.First().Quantity;
                            fillQuantity -= fillsQueue.First().Quantity;
                            fillsQueue.Dequeue();
                        }

                        if (fillQuantity > 0 && fillQuantity < fillsQueue.FirstOrDefault()?.Quantity)
                        {
                            realizedGainLoss += (trade.FillPrice - fillsQueue.First().Price) * fillQuantity;
                            var fill = fillsQueue.First();
                            fill.Quantity -= fillQuantity;
                            fillQuantity = 0;
                        }
                    }
                }
            }

            RealizedGainLoss = realizedGainLoss;
        }

        private void CalculateShortRealizedGainLoss()
        {
            // Calculate gain/loss using FIFO method
            var realizedGainLoss = 0m;
            var fillsQueue = new Queue<FillDetails>();
            foreach (var trade in Trades.OrderBy(t => t.FillDateTime))
            {
                realizedGainLoss -= trade.Commission;
                if (trade.Order.Action == OrderAction.Sell)
                {
                    fillsQueue.Enqueue(new FillDetails(trade.FillDateTime, trade.FillPrice, trade.Commission, trade.FillQuantity));
                }
                else
                {
                    var fillQuantity = trade.FillQuantity;
                    while (fillQuantity > 0)
                    {
                        while (fillQuantity >= fillsQueue.FirstOrDefault()?.Quantity)
                        {
                            realizedGainLoss += (fillsQueue.First().Price - trade.FillPrice) * fillsQueue.First().Quantity;
                            fillQuantity -= fillsQueue.First().Quantity;
                            fillsQueue.Dequeue();
                        }

                        if (fillQuantity > 0 && fillQuantity < fillsQueue.FirstOrDefault().Quantity)
                        {
                            realizedGainLoss += (fillsQueue.First().Price - trade.FillPrice) * fillQuantity;
                            var fill = fillsQueue.First();
                            fill.Quantity -= fillQuantity;
                            fillQuantity = 0;
                        }
                    }
                }
            }

            RealizedGainLoss = realizedGainLoss;
        }
    }
}
