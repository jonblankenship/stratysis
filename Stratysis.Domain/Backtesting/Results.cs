using System;

namespace Stratysis.Domain.Backtesting
{
    public class Results
    {
        private readonly Parameters _parameters;

        public Results(Parameters parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public Parameters Parameters => _parameters;
    }
}
