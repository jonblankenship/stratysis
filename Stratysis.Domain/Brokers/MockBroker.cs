using Stratysis.Domain.Core;
using Stratysis.Domain.Core.Broker;
using Stratysis.Domain.Interfaces;
using Stratysis.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratysis.Domain.Brokers
{
    public class MockBroker: IBroker
    {
        private readonly IAppSettings _settings;
        private readonly List<Account> _accounts = new List<Account>();

        public MockBroker(IAppSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IEnumerable<Account> Accounts => _accounts;

        public Account DefaultAccount => _accounts.FirstOrDefault();

        public void Reset(decimal startingCash)
        {
            _accounts.Clear();
            
            // In the future support multiple accounts in a backtest (i.e., an SP500 comparison account),
            // but for now, we'll just assume one account.
            var defaultAccount = new Account(startingCash);
            _accounts.Add(defaultAccount);
        }

        public void OpenOrder(Account account, Order order)
        {
            account.OpenOrder(order);
        }

        public void OpenOrder(Order order)
        {
            DefaultAccount.OpenOrder(order);
        }

        public void EvaluateOrders(Slice slice)
        {
            foreach (var account in Accounts)
            {
                account.EvaluateOrders(_settings.DefaultCommission, slice);
            }
        }

        public bool HasOpenPosition(string security) => DefaultAccount.HasOpenPosition(security);

        public Position GetOpenPosition(string security) => DefaultAccount.OpenPositions.SingleOrDefault(p => p.Security == security);
    }
}
