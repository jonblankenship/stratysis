using System;

namespace Stratysis.Domain.PositionSizing
{
    public class PositionSizer : IPositionSizer
    {
        private PositionSizingParameters _positionSizingParameters;

        public void Initialize(PositionSizingParameters positionSizingParameters)
        {
            _positionSizingParameters = positionSizingParameters;
        }

        public int GetSize() =>
            GetSize(null, null, null);

        public int GetSize(decimal? entryPrice) =>
            GetSize(entryPrice, null, null);

        public int GetSize(decimal? entryPrice, decimal? accountBalance) =>
            GetSize(entryPrice, accountBalance, null);

        public int GetSize(
            decimal? entryPrice,
            decimal? accountBalance,
            decimal? stopLossPrice)
        {
            if (_positionSizingParameters is null)
                throw new ApplicationException($"{nameof(PositionSizer)}.{nameof(Initialize)} must be called when a strategy is initialized.");

            switch (_positionSizingParameters.Method)
            {
                case PositionSizingMethods.FixedUnits:
                    return CalculateFixedUnitsSize();
                case PositionSizingMethods.FixedDollars:
                    return CalculateFixedDollarsSize(entryPrice);
                case PositionSizingMethods.TradePercentageOfAccountBalance:
                    return CalculateTradePercentageOfAccountBalanceSize(entryPrice, accountBalance);
                case PositionSizingMethods.RiskPercentageOfAccountBalance:
                    return CalculateRiskPercentageOfAccountBalanceSize(entryPrice, stopLossPrice, accountBalance);
                default:
                    throw new ApplicationException("Unknown position sizing method.");
            }
        }

        private int CalculateFixedUnitsSize()
        {
            if (!_positionSizingParameters.Units.HasValue || _positionSizingParameters.Units <= 0)
                throw new ArgumentNullException($"{nameof(_positionSizingParameters.Units)} must be specified on the {nameof(_positionSizingParameters)} if the method is {PositionSizingMethods.FixedUnits}.");

            return _positionSizingParameters.Units.Value;
        }

        private int CalculateFixedDollarsSize(decimal? entryPrice)
        {
            if (!_positionSizingParameters.Dollars.HasValue || _positionSizingParameters.Dollars <= 0)
                throw new ArgumentNullException($"{nameof(_positionSizingParameters.Dollars)} must be specified on the {nameof(_positionSizingParameters)} if the method is {PositionSizingMethods.FixedDollars}.");
            if (!entryPrice.HasValue || entryPrice <= 0)
                throw new ArgumentNullException($"{nameof(entryPrice)} must be specified if the position sizing method is {PositionSizingMethods.FixedDollars}.");

            var units = (int)Math.Floor(_positionSizingParameters.Dollars.Value / entryPrice.Value);
            return units;
        }

        private int CalculateTradePercentageOfAccountBalanceSize(
            decimal? entryPrice,
            decimal? accountBalance)
        {
            if (!_positionSizingParameters.Percent.HasValue || _positionSizingParameters.Dollars <= 0)
                throw new ArgumentNullException($"{nameof(_positionSizingParameters.Percent)} must be specified on the {nameof(_positionSizingParameters)} if the method is {PositionSizingMethods.TradePercentageOfAccountBalance}.");
            if (!entryPrice.HasValue || entryPrice <= 0)
                throw new ArgumentNullException($"{nameof(entryPrice)} must be specified if the position sizing method is {PositionSizingMethods.TradePercentageOfAccountBalance}.");
            if (!accountBalance.HasValue || accountBalance <= 0)
                throw new ArgumentNullException($"{nameof(accountBalance)} must be specified if the position sizing method is {PositionSizingMethods.TradePercentageOfAccountBalance}.");

            var accountPortionToRisk = accountBalance.Value * _positionSizingParameters.Percent.Value;
            var units = (int)Math.Floor(accountPortionToRisk / entryPrice.Value);
            return units;
        }

        private int CalculateRiskPercentageOfAccountBalanceSize(
            decimal? entryPrice,
            decimal? stopLossPrice,
            decimal? accountBalance)
        {
            if (!_positionSizingParameters.Percent.HasValue || _positionSizingParameters.Dollars <= 0)
                throw new ArgumentNullException($"{nameof(_positionSizingParameters.Percent)} must be specified on the {nameof(_positionSizingParameters)} if the method is {PositionSizingMethods.RiskPercentageOfAccountBalance}.");
            if (!entryPrice.HasValue || entryPrice <= 0)
                throw new ArgumentNullException($"{nameof(entryPrice)} must be specified if the position sizing method is {PositionSizingMethods.RiskPercentageOfAccountBalance}.");
            if (!stopLossPrice.HasValue || stopLossPrice <= 0)
                throw new ArgumentNullException($"{nameof(stopLossPrice)} must be specified if the position sizing method is {PositionSizingMethods.RiskPercentageOfAccountBalance}.");
            if (!accountBalance.HasValue || accountBalance <= 0)
                throw new ArgumentNullException($"{nameof(accountBalance)} must be specified if the position sizing method is {PositionSizingMethods.RiskPercentageOfAccountBalance}.");

            var accountPortionToRisk = accountBalance.Value * _positionSizingParameters.Percent.Value;
            var maxLoss = Math.Abs(entryPrice.Value - stopLossPrice.Value);
            var units = (int)Math.Floor(accountPortionToRisk / maxLoss);
            return units;
        }
    }
}
