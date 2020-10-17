namespace Stratysis.Domain.PositionSizing
{
    public class PositionSizingParameters
    {
        public PositionSizingMethods Method { get; set; }

        public int? Units { get; set; }

        public decimal? Dollars { get; set; }
        
        public decimal? Percent { get; set; }
    }
}
