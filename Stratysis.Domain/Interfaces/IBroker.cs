using System.Collections.Generic;
using Stratysis.Domain.Core;
using Stratysis.Domain.Core.Broker;

namespace Stratysis.Domain.Interfaces
{
    public interface IBroker
    {
        IEnumerable<Account> Accounts { get; }

        Account DefaultAccount { get; }

        void Reset(decimal startingCash);

        void OpenOrder(Account account, Order order);

        void OpenOrder(Order order);

        void EvaluateOrders(Slice slice);
    }
}
