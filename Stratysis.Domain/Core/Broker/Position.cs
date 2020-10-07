using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Stratysis.Domain.Core.Broker
{
    /// <summary>
    /// Domain class representing a position in an <see cref="Account"/>
    /// </summary>
    public class Position
    {
        private readonly List<Trade> _trades = new List<Trade>();

        /// <summary>
        /// Instantiates a <see cref="Position"/> established as the result of the given <see cref="order"/> filled
        /// with the given <see cref="FillDetails"/>
        /// </summary>
        /// <param name="order"></param>
        /// <param name="fillDetails"></param>
        public Position(Order order, FillDetails fillDetails)
        {
            Security = order.Security;
            Status = PositionStatus.Open;
            Direction = order.Action == OrderAction.Buy ? PositionDirection.Long : PositionDirection.Short;
            PositionSize = fillDetails.Quantity;
            MaxPositionSize = PositionSize;
            OpenDateTime = fillDetails.DateTime;
            
            _trades.Add(new Trade(order, fillDetails));
            
            order.Status = OrderStatus.Executed;

            CalculateRealizedGainLoss();
        }

        /// <summary>
        /// The date this <see cref="Position"/> was opened
        /// </summary>
        public DateTime OpenDateTime { get; }

        /// <summary>
        /// The date this <see cref="Position"/> was opened
        /// </summary>
        public DateTime? CloseDateTime { get; private set; }

        /// <summary>
        /// The symbol of the security for this <see cref="Position"/>
        /// </summary>
        public string Security { get; }

        /// <summary>
        /// The <see cref="PositionStatus"/>
        /// </summary>
        public PositionStatus Status { get; private set; }

        /// <summary>
        /// The <see cref="PositionDirection"/>
        /// </summary>
        public PositionDirection Direction { get; }

        /// <summary>
        /// The realized gain/loss of this <see cref="Position"/>
        /// </summary>
        public decimal RealizedGainLoss => Trades.Sum(t => t.RealizedGainLoss);

        /// <summary>
        /// The realized gain/loss of this <see cref="Position"/> in points
        /// </summary>
        public decimal RealizedGainLossPoints => Trades.Sum(t => t.RealizedGainLossPoints);

        /// <summary>
        /// The size of this <see cref="Position"/>
        /// </summary>
        public int PositionSize { get; private set; }

        /// <summary>
        /// The maximum size of this <see cref="Position"/> during its lifetime
        /// </summary>
        public int MaxPositionSize { get; private set; }

        /// <summary>
        /// The <see cref="Trade"/>s executed to establish, increase, reduce or close this <see cref="Position"/>
        /// </summary>
        public IEnumerable<Trade> Trades => _trades;

        /// <summary>
        /// The <see cref="Trade"/>s executed to reduce or close this <see cref="Position"/>
        /// </summary>
        public IEnumerable<Trade> ClosedTrades => _trades.Where(t => t.IsExitTrade);

        /// <summary>
        /// Instantiates a <see cref="Position"/> established as the result of the given <see cref="order"/> filled
        /// with the given <see cref="FillDetails"/>
        /// </summary>
        /// <param name="order"></param>
        /// <param name="fillDetails"></param>
        public static Position InitiatePosition(Order order, FillDetails fillDetails) => new Position(order, fillDetails);

        /// <summary>
        /// Fills the given <see cref="order"/> with the <see cref="fillDetails"/> against an instance of this <see cref="Position"/>
        /// </summary>
        /// <param name="order"></param>
        /// <param name="fillDetails"></param>
        /// <returns></returns>
        public Trade FillOrder(Order order, FillDetails fillDetails)
        {
            Trade newTrade;
            if ((Direction == PositionDirection.Long && order.Action == OrderAction.Buy) ||
                (Direction == PositionDirection.Short && order.Action == OrderAction.Sell))
            {
                // Adding to position
                newTrade = new Trade(order, fillDetails);
                _trades.Add(newTrade);
                PositionSize += fillDetails.Quantity;
                MaxPositionSize = Math.Max(PositionSize, MaxPositionSize);
            }
            else
            {
                // Reducing or closing position
                if (fillDetails.Quantity > PositionSize)
                    throw new ApplicationException("Fill quantity exceeds position size.");

                newTrade = new Trade(order, fillDetails, true);
                _trades.Add(newTrade);
                PositionSize -= fillDetails.Quantity;
                if (PositionSize == 0)
                {
                    Status = PositionStatus.Closed;
                    CloseDateTime = fillDetails.DateTime;
                }
            }

            order.Status = OrderStatus.Executed;

            CalculateRealizedGainLoss();

            if (Status == PositionStatus.Closed)
                Debug.WriteLine($"{fillDetails.DateTime} Position closed: {RealizedGainLoss}");

            return newTrade;
        }

        /// <summary>
        /// Calculates the unrealized gain/loss of an instance of this <see cref="Position"/> as of the <see cref="asOfSlice"/>
        /// </summary>
        /// <param name="asOfSlice">The slice as of which to calculate the unrealized gain/loss</param>
        /// <returns></returns>
        public decimal GetUnrealizedGainLoss(Slice asOfSlice)
        {
            var asOfBar = asOfSlice[Security].Bar;
            return GetUnrealizedGainLoss(asOfBar);
        }

        /// <summary>
        /// Calculates the unrealized gain/loss of an instance of this <see cref="Position"/> as of the <see cref="asOfBar"/>
        /// </summary>
        /// <param name="asOfBar">The bar as of which to calculate the unrealized gain/loss</param>
        /// <returns></returns>
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
            var fillsQueue = new Queue<FillDetails>();
            foreach (var trade in Trades.OrderBy(t => t.FillDateTime))
            {
                if (trade.Order.Action == OrderAction.Buy)
                {
                    trade.RealizedGainLossPoints = 0;
                    trade.RealizedGainLoss = 0 - trade.Commission;
                    fillsQueue.Enqueue(new FillDetails(trade.FillDateTime, trade.FillPrice, trade.Commission, trade.FillQuantity));
                }
                else
                {
                    var fillQuantity = trade.FillQuantity;
                    while (fillQuantity > 0)
                    {
                        while (fillQuantity >= fillsQueue.FirstOrDefault()?.Quantity)
                        {
                            trade.RealizedGainLossPoints = trade.FillPrice - fillsQueue.First().Price;
                            trade.RealizedGainLoss = trade.RealizedGainLossPoints * fillsQueue.First().Quantity - trade.Commission;
                            fillQuantity -= fillsQueue.First().Quantity;
                            fillsQueue.Dequeue();
                        }

                        if (fillQuantity > 0 && fillQuantity < fillsQueue.FirstOrDefault()?.Quantity)
                        {
                            trade.RealizedGainLossPoints = trade.FillPrice - fillsQueue.First().Price;
                            trade.RealizedGainLoss = trade.RealizedGainLossPoints * fillQuantity - trade.Commission;
                            var fill = fillsQueue.First();
                            fill.Quantity -= fillQuantity;
                            fillQuantity = 0;
                        }
                    }
                }
            }
        }

        private void CalculateShortRealizedGainLoss()
        {
            // Calculate gain/loss using FIFO method
            var fillsQueue = new Queue<FillDetails>();
            foreach (var trade in Trades.OrderBy(t => t.FillDateTime))
            {
                if (trade.Order.Action == OrderAction.Sell)
                {
                    trade.RealizedGainLossPoints = 0;
                    trade.RealizedGainLoss = 0 - trade.Commission;
                    fillsQueue.Enqueue(new FillDetails(trade.FillDateTime, trade.FillPrice, trade.Commission, trade.FillQuantity));
                }
                else
                {
                    var fillQuantity = trade.FillQuantity;
                    while (fillQuantity > 0)
                    {
                        while (fillQuantity >= fillsQueue.FirstOrDefault()?.Quantity)
                        {
                            trade.RealizedGainLossPoints = fillsQueue.First().Price - trade.FillPrice;
                            trade.RealizedGainLoss = trade.RealizedGainLossPoints * fillsQueue.First().Quantity - trade.Commission;
                            fillQuantity -= fillsQueue.First().Quantity;
                            fillsQueue.Dequeue();
                        }

                        if (fillQuantity > 0 && fillQuantity < fillsQueue.FirstOrDefault().Quantity)
                        {
                            trade.RealizedGainLossPoints = fillsQueue.First().Price - trade.FillPrice;
                            trade.RealizedGainLoss = trade.RealizedGainLossPoints * fillQuantity - trade.Commission;
                            var fill = fillsQueue.First();
                            fill.Quantity -= fillQuantity;
                            fillQuantity = 0;
                        }
                    }
                }
            }
        }
    }
}
