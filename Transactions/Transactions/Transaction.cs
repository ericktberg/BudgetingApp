using Newtonsoft.Json;
using System;
using System.Configuration;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Transactions
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Transaction
    {
        public Transaction(decimal amount) : this(amount, Guid.NewGuid())
        {
        }

        [JsonConstructor]
        public Transaction(decimal amount, Guid guid)
        {
            Amount = amount;
            TransactionGuid = guid;
        }

        [JsonProperty]
        public Guid TransactionGuid { get; }

        [JsonProperty]
        public decimal Amount { get; set; }

        public virtual decimal Value => Amount;

        public override bool Equals(object obj)
        {
            var transaction = obj as Transaction;
            return transaction != null &&
                   TransactionGuid.Equals(transaction.TransactionGuid);
        }
    }
}