using Newtonsoft.Json;

namespace Transactions.Accounts
{
    public enum AccountType
    {
        Liquid,
        Invested,
        Debt,
        Property,
    }

    public class Account
    {
        public Account(string name) : this(name, AccountType.Liquid)
        {
        }

        [JsonConstructor]
        public Account(string name, AccountType type)
        {
            Name = name;
            Type = type;
        }

        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public AccountType Type { get; private set; }

        public override bool Equals(object obj)
        {
            var account = obj as Account;
            return account != null &&
                   Name == account.Name;
        }

        public virtual decimal GetDepositDelta(decimal amount)
        {
            return amount;
        }

        public virtual decimal GetWithdrawDelta(decimal amount)
        {
            return -amount;
        }
    }
}