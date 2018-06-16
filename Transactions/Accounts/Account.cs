using System;
using System.Collections.Generic;

namespace Transactions.Accounts
{
    public class Account
    {
        public Account(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

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