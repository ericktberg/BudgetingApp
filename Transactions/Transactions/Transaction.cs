using Newtonsoft.Json;
using System;

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
        public decimal Amount { get; set; }

        [JsonProperty]
        public Guid TransactionGuid { get; }

        public virtual decimal Value => Amount;

        public override bool Equals(object obj)
        {
            var transaction = obj as Transaction;
            return transaction != null &&
                   TransactionGuid.Equals(transaction.TransactionGuid);
        }
    }
}