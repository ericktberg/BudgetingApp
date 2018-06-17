using Newtonsoft.Json;
using System;
using System.Configuration;
using Transactions.Accounts;

namespace Transactions
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Transaction
    {
        public Transaction(decimal amount, Account fromAccount, Account toAccount)
        {
            AccountWithdrawnFrom = fromAccount;
            AccountDepositedTo = toAccount;
            Amount = amount;
        }

        [JsonProperty]
        public Account AccountDepositedTo { get; private set; }

        [JsonProperty]
        public Account AccountWithdrawnFrom { get; private set; }

        [JsonProperty]
        public decimal Amount { get; set; }

        public virtual decimal GetValue(Account account)
        {
            if (AccountDepositedTo?.Equals(account) ?? false)
            {
                return account.GetDepositDelta(Amount);
            }
            else if (AccountWithdrawnFrom?.Equals(account) ?? false)
            {
                return account.GetWithdrawDelta(Amount);
            }
            else
            {
                throw new ArgumentException("Irrelevant account encountered", nameof(account));
            }
        }
    }
}