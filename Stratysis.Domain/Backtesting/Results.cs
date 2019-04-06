using System;
using System.Diagnostics;
using System.Linq;
using Stratysis.Domain.Core;
using Stratysis.Domain.Core.Broker;

namespace Stratysis.Domain.Backtesting
{
    public class Results
    {
        private readonly BacktestParameters _parameters;

        public Results(BacktestParameters parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public void CalculateResults(Account account, Slice asOfSlice)
        {
            TotalRealizedGainLoss = account.Positions.Sum(p => p.RealizedGainLoss);
            TotalUnrealizedGainLoss = account.Positions.Sum(p => p.GetUnrealizedGainLoss(asOfSlice));
            TotalClosedTrades = account.Positions.Sum(p => p.ClosedTrades.Count());
            RemainingOpenPositions = account.Positions.Count(p => p.Status == PositionStatus.Open);
            Wins = account.Positions.Sum(p => p.ClosedTrades.Count(t => t.RealizedGainLoss > 0));
            Losses = account.Positions.Sum(p => p.ClosedTrades.Count(t => t.RealizedGainLoss <= 0));
            WinPercentage = (decimal)Wins / TotalClosedTrades;
            Expectancy = account.Positions.Sum(p => p.ClosedTrades.Sum(t => t.RealizedGainLoss)) / TotalClosedTrades;
            AverageWin = account.Positions.Sum(p => p.ClosedTrades.Where(t => t.RealizedGainLoss > 0).Sum(t => t.RealizedGainLoss)) / Wins;
            AverageLoss = account.Positions.Sum(p => p.ClosedTrades.Where(t => t.RealizedGainLoss <= 0).Sum(t => t.RealizedGainLoss)) / Losses;
            GainLossPercentage = (account.AccountBalance - _parameters.StartingCash) / _parameters.StartingCash;
            FinalAccountBalance = account.AccountBalance;
        }

        public BacktestParameters Parameters => _parameters;

        public int TotalClosedTrades { get; private set; }

        public int RemainingOpenPositions { get; private set; }

        public int Wins { get; private set; }

        public int Losses { get; private set; }
        
        public decimal WinPercentage { get; private set; }

        public decimal Expectancy { get; private set; }

        public decimal AverageWin { get; private set; }

        public decimal AverageLoss { get; private set; }

        public decimal TotalRealizedGainLoss { get; private set; }        

        public decimal TotalUnrealizedGainLoss { get; private set; }

        public decimal GainLossPercentage { get; private set; }

        public decimal FinalAccountBalance { get; private set; }

        public override string ToString()
        {
            return $"{Environment.NewLine}{Environment.NewLine}Results{Environment.NewLine}" +
                   $"-------{Environment.NewLine}" +
                   $"Total Closed Trades:      {TotalClosedTrades}{Environment.NewLine}" +
                   $"Expectancy:               {Expectancy:C}{Environment.NewLine}" +
                   $"Wins/Losses:              {Wins} / {Losses} ({WinPercentage:P2}){Environment.NewLine}" +
                   $"Avg Win/Loss:             {AverageWin:C} / {AverageLoss:C}{Environment.NewLine}" +
                   $"Realized Gain:            {TotalRealizedGainLoss:C} ({GainLossPercentage:P2}){Environment.NewLine}" +
                   $"Final Balance:            {FinalAccountBalance:C}{Environment.NewLine}{Environment.NewLine}" +
                   $"Remaining Open Positions: {RemainingOpenPositions}{Environment.NewLine}" +
                   $"Unrealized Gain:          {TotalUnrealizedGainLoss:C}{Environment.NewLine}{Environment.NewLine}";
        }
    }
}
