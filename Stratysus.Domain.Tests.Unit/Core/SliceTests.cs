using FluentAssertions;
using Stratysis.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Stratysus.Domain.Tests.Unit.Core
{
    /// <summary>
    /// Unit tests for <see cref="Slice"/>
    /// </summary>
    public class SliceTests
    {
        private readonly DateTime _sliceDate = DateTime.Now;
        private readonly decimal AaplHigh = 123.34m;
        private readonly decimal MsftHigh = 99.43m;

        [Fact]
        public void IntegerIndexer_RecentSlicesEmpty_Zero_CurrentSliceReturned()
        {
            // Arrange
            var slice = new Slice(null);

            // Act
            var result = slice[0];

            // Assert
            result.Should().Be(slice);
        }

        [Fact]
        public void IntegerIndexer_RecentSlicesEmpty_NegativeOne_NullReturned()
        {
            // Arrange
            var slice = new Slice(null);

            // Act
            var result = slice[-1];

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void IntegerIndexer_RecentSlicesEmpty_One_ArgumentException()
        {
            // Arrange
            var slice = new Slice(null);

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() => slice[1]);
        }

        [Fact]
        public void IntegerIndexer_RecentSlicesLessThanQueueCapacity_NegativeSliceCount_FirstSliceReturned()
        {
            // Arrange
            var prevSlicesCount = 5;
            var slice = GenerateSlice(prevSlicesCount);

            // Act
            var result = slice[0 - prevSlicesCount];

            // Assert
            result.Should().Be(slice.RecentSlices.First());
        }

        [Fact]
        public void IntegerIndexer_RecentSlicesLessThanQueueCapacity_NegativeSliceCountMinusOne_NullReturned()
        {
            // Arrange
            var prevSlicesCount = 5;
            var slice = GenerateSlice(prevSlicesCount);

            // Act
            var result = slice[0 - prevSlicesCount - 1];

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void IntegerIndexer_RecentSlicesMoreThanQueueCapacity_NegativeSliceCount_FirstSliceReturned()
        {
            // Arrange
            var prevSlicesCount = 25;
            var slice = GenerateSlice(prevSlicesCount);
            var firstSlice = slice.RecentSlices.First().RecentSlices.First();

            // Act
            var result = slice[0 - prevSlicesCount];

            // Assert
            result.Should().Be(firstSlice);
        }

        [Fact]
        public void IntegerIndexer_RecentSlicesMoreThanQueueCapacity_NegativeSliceCountMinusOne_NullReturned()
        {
            // Arrange
            var prevSlicesCount = 25;
            var slice = GenerateSlice(prevSlicesCount);

            // Act
            var result = slice[0 - prevSlicesCount - 1];

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void StringIndexer_InvalidSymbol_NullReturned()
        {
            // Arrange
            var slice = GenerateSlice(0);

            // Act
            var result = slice["V"];

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void StringIndexer_ValidSymbol_CurrentSlice_SecuritySliceReturned()
        {
            // Arrange
            var slice = GenerateSlice(0);

            // Act
            var result = slice["AAPL"];

            // Assert
            result.DateTime.Should().Be(_sliceDate);
            result.High.Should().Be(AaplHigh);
        }

        [Fact]
        public void StringIndexer_ValidSymbol_CurrentSliceMinus5_SecuritySliceReturned()
        {
            // Arrange
            var slice = GenerateSlice(10);

            // Act
            var result = slice["AAPL"][-5];

            // Assert
            result.DateTime.Should().Be(_sliceDate.AddDays(-5));
            result.High.Should().Be(AaplHigh);
        }

        private Slice GenerateSlice(int prevSlicesCount)
        {
            return new Slice(GeneratePreviousSlice(prevSlicesCount))
                {
                    DateTime = _sliceDate,
                    Bars = new Dictionary<string, Bar>
                    {
                        { "AAPL", new Bar { High = AaplHigh } },
                        { "MSFT", new Bar { High = MsftHigh } }
                    }
                };
        }

        private Slice GeneratePreviousSlice(int prevSlicesCount)
        {
            if (prevSlicesCount == 0)
                return null;

            var startingDateTime = _sliceDate.AddDays(0 - prevSlicesCount);
            Slice slice = null;
            while (startingDateTime < _sliceDate)
            {
                slice = new Slice(slice)
                {
                    DateTime = startingDateTime,
                    Bars = new Dictionary<string, Bar>
                    {
                        { "AAPL", new Bar { High = AaplHigh } },
                        { "MSFT", new Bar { High = MsftHigh } }
                    }
                };
                startingDateTime = startingDateTime.AddDays(1);
            }

            return slice;
        }
    }
}
