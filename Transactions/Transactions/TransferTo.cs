using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Transactions.Accounts;

namespace Transactions
{
    public class TransferTo : Transaction
    {
        public TransferTo(decimal amount, Account withdrawnFrom) 
            : base(amount)
        {
            AccountWithdrawnFrom = withdrawnFrom;
        }

        [JsonConstructor]
        public TransferTo(decimal amount, Account withdrawnFrom, Guid guid) : base(amount, guid)
        {
            AccountWithdrawnFrom = withdrawnFrom;
        }

        [JsonProperty]
        public Account AccountWithdrawnFrom { get; private set; }

        public override decimal Value => base.Value;
    }
}
