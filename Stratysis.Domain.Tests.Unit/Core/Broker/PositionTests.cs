using FluentAssertions;
using Stratysis.Domain.Core.Broker;
using System;
using System.Linq;
using Stratysis.Domain.Core;
using Xunit;

namespace Stratysis.Domain.Tests.Unit.Core.Broker
{
    public class PositionTests
    {
        private const decimal Commission = 6.95m;

        [Fact]
        public void InitiatePosition_BuyOrder_LongPositionInitiated()
        {
            // Arrange
            var order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Buy,
                Quantity = 100,
                Security = "MSFT"
            };
            var fillDetails = new FillDetails(DateTime.UtcNow, 55.45m, Commission, 100);

            // Act
            var position = Position.InitiatePosition(order, fillDetails);

            // Assert
            position.Security.Should().Be(order.Security);
            position.Status.Should().Be(PositionStatus.Open);
            position.Direction.Should().Be(PositionDirection.Long);
            position.PositionSize.Should().Be(fillDetails.Quantity);
            position.Trades.Count().Should().Be(1);
            position.RealizedGainLoss.Should().Be(0 - Commission);
        }

        [Fact]
        public void InitiatePosition_SellOrder_ShortPositionInitiated()
        {
            // Arrange
            var order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Sell,
                Quantity = 100,
                Security = "MSFT"
            };
            var fillDetails = new FillDetails(DateTime.UtcNow, 55.45m, Commission, 100);

            // Act
            var position = Position.InitiatePosition(order, fillDetails);

            // Assert
            position.Security.Should().Be(order.Security);
            position.Status.Should().Be(PositionStatus.Open);
            position.Direction.Should().Be(PositionDirection.Short);
            position.PositionSize.Should().Be(fillDetails.Quantity);
            position.Trades.Count().Should().Be(1);
            position.RealizedGainLoss.Should().Be(0 - Commission);
        }

        [Fact]
        public void FillOrder_LongPosition_BuyOrder_PositionSizeIncreased_TradeAdded()
        {
            // Arrange
            var order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Buy,
                Quantity = 100,
                Security = "MSFT"
            };
            var fillDetails = new FillDetails(DateTime.UtcNow, 55.45m, Commission, 100);
            var position = Position.InitiatePosition(order, fillDetails);

            var newOrder = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Buy,
                Quantity = 75,
                Security = "MSFT"
            };
            var newFillDetails = new FillDetails(DateTime.UtcNow, 56.12m, Commission, 75);

            // Act
            position.FillOrder(newOrder, newFillDetails);

            // Assert
            position.Security.Should().Be(order.Security);
            position.Status.Should().Be(PositionStatus.Open);
            position.Direction.Should().Be(PositionDirection.Long);
            position.PositionSize.Should().Be(fillDetails.Quantity + newFillDetails.Quantity);
            position.Trades.Count(t => t.Order.Action == OrderAction.Buy).Should().Be(2);
            position.Trades.Count(t => t.Order.Action == OrderAction.Sell).Should().Be(0);
            position.RealizedGainLoss.Should().Be(0 - 2 * Commission);
        }

        [Fact]
        public void FillOrder_LongPosition_SellOrderForPartialPosition_PositionSizeDecreased_TradeAdded()
        {
            // Arrange
            var order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Buy,
                Quantity = 100,
                Security = "MSFT"
            };
            var fillDetails = new FillDetails(DateTime.UtcNow, 55.45m, Commission, 100);
            var position = Position.InitiatePosition(order, fillDetails);

            var newOrder = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Sell,
                Quantity = 75,
                Security = "MSFT"
            };
            var newFillDetails = new FillDetails(DateTime.UtcNow, 56.12m, Commission, 75);

            // Act
            position.FillOrder(newOrder, newFillDetails);

            // Assert
            position.Security.Should().Be(order.Security);
            position.Status.Should().Be(PositionStatus.Open);
            position.Direction.Should().Be(PositionDirection.Long);
            position.PositionSize.Should().Be(fillDetails.Quantity - newFillDetails.Quantity);
            position.Trades.Count(t => t.Order.Action == OrderAction.Buy).Should().Be(1);
            position.Trades.Count(t => t.Order.Action == OrderAction.Sell).Should().Be(1);
            position.RealizedGainLoss.Should().Be(0 - 2 * Commission + (56.12m - 55.45m) * 75);
        }

        [Fact]
        public void FillOrder_LongPosition_SellOrderToClosePosition_PositionClosed_TradeAdded()
        {
            // Arrange
            var order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Buy,
                Quantity = 100,
                Security = "MSFT"
            };
            var fillDetails = new FillDetails(DateTime.UtcNow, 55.45m, Commission, 100);
            var position = Position.InitiatePosition(order, fillDetails);

            var newOrder = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Sell,
                Quantity = 100,
                Security = "MSFT"
            };
            var newFillDetails = new FillDetails(DateTime.UtcNow, 56.12m, Commission, 100);

            // Act
            position.FillOrder(newOrder, newFillDetails);

            // Assert
            position.Security.Should().Be(order.Security);
            position.Status.Should().Be(PositionStatus.Closed);
            position.Direction.Should().Be(PositionDirection.Long);
            position.PositionSize.Should().Be(0);
            position.Trades.Count(t => t.Order.Action == OrderAction.Buy).Should().Be(1);
            position.Trades.Count(t => t.Order.Action == OrderAction.Sell).Should().Be(1);
            position.RealizedGainLoss.Should().Be(0 - 2 * Commission + (56.12m - 55.45m) * 100);
        }
        
        [Fact]
        public void FillOrder_ShortPosition_SellOrder_PositionSizeIncreased_TradeAdded()
        {
            // Arrange
            var order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Sell,
                Quantity = 100,
                Security = "MSFT"
            };
            var fillDetails = new FillDetails(DateTime.UtcNow, 55.45m, Commission, 100);
            var position = Position.InitiatePosition(order, fillDetails);

            var newOrder = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Sell,
                Quantity = 75,
                Security = "MSFT"
            };
            var newFillDetails = new FillDetails(DateTime.UtcNow, 56.12m, Commission, 75);

            // Act
            position.FillOrder(newOrder, newFillDetails);

            // Assert
            position.Security.Should().Be(order.Security);
            position.Status.Should().Be(PositionStatus.Open);
            position.Direction.Should().Be(PositionDirection.Short);
            position.PositionSize.Should().Be(fillDetails.Quantity + newFillDetails.Quantity);
            position.Trades.Count(t => t.Order.Action == OrderAction.Sell).Should().Be(2);
            position.Trades.Count(t => t.Order.Action == OrderAction.Buy).Should().Be(0);
            position.RealizedGainLoss.Should().Be(0 - 2 * Commission);
        }

        [Fact]
        public void FillOrder_ShortPosition_BuyOrderForPartialPosition_PositionSizeDecreased_TradeAdded()
        {
            // Arrange
            var order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Sell,
                Quantity = 100,
                Security = "MSFT"
            };
            var fillDetails = new FillDetails(DateTime.UtcNow, 55.45m, Commission, 100);
            var position = Position.InitiatePosition(order, fillDetails);

            var newOrder = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Buy,
                Quantity = 75,
                Security = "MSFT"
            };
            var newFillDetails = new FillDetails(DateTime.UtcNow, 56.12m, Commission, 75);

            // Act
            position.FillOrder(newOrder, newFillDetails);

            // Assert
            position.Security.Should().Be(order.Security);
            position.Status.Should().Be(PositionStatus.Open);
            position.Direction.Should().Be(PositionDirection.Short);
            position.PositionSize.Should().Be(fillDetails.Quantity - newFillDetails.Quantity);
            position.Trades.Count(t => t.Order.Action == OrderAction.Buy).Should().Be(1);
            position.Trades.Count(t => t.Order.Action == OrderAction.Sell).Should().Be(1);
            position.RealizedGainLoss.Should().Be(0 - 2 * Commission + (55.45m - 56.12m) * 75);
        }

        [Fact]
        public void FillOrder_ShortPosition_BuyOrderToClosePosition_PositionClosed_TradeAdded()
        {
            // Arrange
            var order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Sell,
                Quantity = 100,
                Security = "MSFT"
            };
            var fillDetails = new FillDetails(DateTime.UtcNow, 55.45m, Commission, 100);
            var position = Position.InitiatePosition(order, fillDetails);

            var newOrder = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Buy,
                Quantity = 100,
                Security = "MSFT"
            };
            var newFillDetails = new FillDetails(DateTime.UtcNow, 56.12m, Commission, 100);

            // Act
            position.FillOrder(newOrder, newFillDetails);

            // Assert
            position.Security.Should().Be(order.Security);
            position.Status.Should().Be(PositionStatus.Closed);
            position.Direction.Should().Be(PositionDirection.Short);
            position.PositionSize.Should().Be(0);
            position.Trades.Count(t => t.Order.Action == OrderAction.Buy).Should().Be(1);
            position.Trades.Count(t => t.Order.Action == OrderAction.Sell).Should().Be(1);
            position.RealizedGainLoss.Should().Be(0 - 2 * Commission + (55.45m - 56.12m) * 100);
        }

        [Fact]
        public void CalculateUnrealizedGainLoss_LongPosition_MultipleLegs_CorrectAmountReturned()
        {
            // Arrange
            // First leg
            var order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Buy,
                Quantity = 150,
                Security = "MSFT"
            };
            var fillDetails = new FillDetails(DateTime.UtcNow, 55.45m, Commission, 150);
            var position = Position.InitiatePosition(order, fillDetails);

            // Second leg
            order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Buy,
                Quantity = 75,
                Security = "MSFT"
            };
            fillDetails = new FillDetails(DateTime.UtcNow, 56.12m, Commission, 75);
            position.FillOrder(order, fillDetails);

            // Take partial profit
            order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Sell,
                Quantity = 100,
                Security = "MSFT"
            };
            fillDetails = new FillDetails(DateTime.UtcNow, 58.22m, Commission, 100);
            position.FillOrder(order, fillDetails);

            var asOfBar = new Bar { Close = 59.21m };

            var expectedAmount = 50 * (asOfBar.Close - 55.45m) + 75 * (asOfBar.Close - 56.12m);

            // Act
            var result = position.GetUnrealizedGainLoss(asOfBar);

            // Assert
            result.Should().Be(expectedAmount);
        }

        [Fact]
        public void CalculateUnrealizedGainLoss_ShortPosition_MultipleLegs_CorrectAmountReturned()
        {
            // Arrange
            // First leg
            var order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Sell,
                Quantity = 150,
                Security = "MSFT"
            };
            var fillDetails = new FillDetails(DateTime.UtcNow, 55.45m, Commission, 150);
            var position = Position.InitiatePosition(order, fillDetails);

            // Second leg
            order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Buy,
                Quantity = 75,
                Security = "MSFT"
            };
            fillDetails = new FillDetails(DateTime.UtcNow, 56.12m, Commission, 75);
            position.FillOrder(order, fillDetails);

            // Take partial profit
            order = new Order
            {
                Type = OrderTypes.Market,
                Action = OrderAction.Sell,
                Quantity = 100,
                Security = "MSFT"
            };
            fillDetails = new FillDetails(DateTime.UtcNow, 51.22m, Commission, 100);
            position.FillOrder(order, fillDetails);

            var asOfBar = new Bar { Close = 52.16m };

            var expectedAmount = 75 * (55.45m - asOfBar.Close) + 100 * (51.22m - asOfBar.Close);

            // Act
            var result = position.GetUnrealizedGainLoss(asOfBar);

            // Assert
            result.Should().Be(expectedAmount);
        }
    }
}
