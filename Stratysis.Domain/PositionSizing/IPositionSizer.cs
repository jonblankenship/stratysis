namespace Stratysis.Domain.PositionSizing
{
    public interface IPositionSizer
    {
        void Initialize(PositionSizingParameters positionSizingParameters);

        public int GetSize();

        public int GetSize(decimal? entryPrice);

        public int GetSize(decimal? entryPrice, decimal? accountBalance);

        public int GetSize(
            decimal? entryPrice,
            decimal? accountBalance,
            decimal? stopLossPrice);
    }
}
