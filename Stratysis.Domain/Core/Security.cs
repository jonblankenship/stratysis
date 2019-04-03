namespace Stratysis.Domain.Core
{
    public class Security
    {
        public Security(string symbol)
        {
            Symbol = symbol;
        }

        public string Symbol { get; }

        public bool IsActive { get; set; } = true;
    }
}
