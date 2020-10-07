using System;
using System.Collections.Generic;
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
            StartingAccountBalance = parameters.StartingCash;
        }

        public void CalculateResults(Account account, Slice asOfSlice)
        {
            TotalRealizedGainLoss = account.Positions.Sum(p => p.RealizedGainLoss);
            TotalRealizedGainLossPoints = account.Positions.Sum(p => p.RealizedGainLossPoints);
            TotalUnrealizedGainLoss = account.Positions.Sum(p => p.GetUnrealizedGainLoss(asOfSlice));
            TotalClosedTrades = account.Positions.Sum(p => p.ClosedTrades.Count());
            RemainingOpenPositions = account.Positions.Count(p => p.Status == PositionStatus.Open);
            Wins = account.Positions.Sum(p => p.ClosedTrades.Count(t => t.RealizedGainLoss > 0));
            Losses = account.Positions.Sum(p => p.ClosedTrades.Count(t => t.RealizedGainLoss <= 0));
            WinPercentage = TotalClosedTrades == 0 ? 0 : (decimal)Wins / TotalClosedTrades;
            Expectancy = TotalClosedTrades == 0 ? 0 : account.Positions.Sum(p => p.ClosedTrades.Sum(t => t.RealizedGainLoss)) / TotalClosedTrades;
            AverageWin = Wins == 0 ? 0 : account.Positions.Sum(p => p.ClosedTrades.Where(t => t.RealizedGainLoss > 0).Sum(t => t.RealizedGainLoss)) / Wins;
            AverageLoss = Losses == 0 ? 0 : account.Positions.Sum(p => p.ClosedTrades.Where(t => t.RealizedGainLoss <= 0).Sum(t => t.RealizedGainLoss)) / Losses;
            GainLossPercentage = _parameters.StartingCash == 0 ? 0 : (account.AccountBalance - _parameters.StartingCash) / _parameters.StartingCash;
            FinalAccountBalance = account.AccountBalance;

            Positions = account.Positions.ToList();

            CalculateAccountBalanceSeries();
        }

        private void CalculateAccountBalanceSeries()
        {
            AccountBalanceSeries.Clear();
            AccountBalanceSeries.Add(new KeyValuePair<DateTime, decimal>(_parameters.StartDateTime, StartingAccountBalance));
            var lastBalance = StartingAccountBalance;
            foreach (var p in Positions.Where(x => x.Status == PositionStatus.Closed))
            {
                lastBalance += p.RealizedGainLoss;
                AccountBalanceSeries.Add(new KeyValuePair<DateTime, decimal>(p.CloseDateTime.Value, lastBalance));
            }

            AccountBalanceSeries.Add(new KeyValuePair<DateTime, decimal>(_parameters.EndDateTime, FinalAccountBalance));
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

        public decimal TotalRealizedGainLossPoints { get; private set; }

        public decimal TotalUnrealizedGainLoss { get; private set; }

        public decimal GainLossPercentage { get; private set; }

        public decimal StartingAccountBalance { get; private set; }

        public decimal FinalAccountBalance { get; private set; }

        public List<Position> Positions { get; private set; }

        public List<KeyValuePair<DateTime, decimal>> AccountBalanceSeries { get; set; } = new List<KeyValuePair<DateTime, decimal>>();

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
