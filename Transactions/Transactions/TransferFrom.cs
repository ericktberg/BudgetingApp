using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Accounts;

namespace Transactions
{
    public class TransferFrom : Transaction
    {
        public TransferFrom(decimal amount, Account depositedTo)
          : base(amount)
        {
            AccountDepositedTo = depositedTo;
        }

        [JsonConstructor]
        public TransferFrom(decimal amount, Account depositedTo, Guid guid) : base(amount, guid)
        {
            AccountDepositedTo = depositedTo;
        }

        [JsonProperty]
        public Account AccountDepositedTo { get; private set; }

        public override decimal Value => -base.Value;
    }
}
