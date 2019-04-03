using Stratysis.Domain.Core;
using System;

namespace Stratysis.Domain.Backtesting
{
    public class Progress
    {
        private readonly Parameters _parameters;
        private Slice _lastSlice;

        public Progress(Parameters parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public DateTime? LastDateTimeProcessed => _lastSlice?.DateTime;

        public decimal PercentComplete
        {
            get
            {
                if (_lastSlice == null)
                    return 0;

                decimal totalTicks = _parameters.EndDateTime.Ticks - _parameters.StartDateTime.Ticks;
                decimal elapsedTicks = _lastSlice.DateTime.Ticks - _parameters.StartDateTime.Ticks;
                return Math.Round(elapsedTicks / totalTicks, 2);
            }
        } 
        
        public void ProcessSlice(Slice slice)
        {
            var lastPercentage = PercentComplete;
            _lastSlice = slice;
            if (Math.Floor(PercentComplete * 100) % 5 == 0 && Math.Floor(PercentComplete * 100) != Math.Floor(lastPercentage * 100))
            {
                ProgressChanged?.Invoke(this, null);
            }
        }

        public event EventHandler ProgressChanged;
    }
}