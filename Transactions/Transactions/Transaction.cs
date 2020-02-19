using Newtonsoft.Json;
using System;

namespace Sunsets.Transactions
{
    public interface ITransaction
    {
        /// <summary>
        /// The effect the transaction would have on assets. Improving assets is positive, adding liabilities is negative.
        /// </summary>
        decimal Value { get; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Transaction : ITransaction
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
        public Guid TransactionGuid { get; set; }

        public virtual decimal Value => Amount;

        public override bool Equals(object obj)
        {
            var transaction = obj as Transaction;
            return transaction != null &&
                   TransactionGuid.Equals(transaction.TransactionGuid);
        }
    }
}