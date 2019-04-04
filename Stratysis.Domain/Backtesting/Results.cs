using System;

namespace Stratysis.Domain.Backtesting
{
    public class Results
    {
        private readonly BacktestParameters _parameters;

        public Results(BacktestParameters parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public BacktestParameters Parameters => _parameters;
    }
}
