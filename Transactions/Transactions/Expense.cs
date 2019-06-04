using Newtonsoft.Json;
using System;
using System.Configuration;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Transactions
{
    public enum ExpenseCategory
    {
        Living,
        Groceries,
        Exercise,
        SelfCare,
        Subscriptions,
        Travel,
        Things,
        Entertainment,
        WealthManagement,
    }

    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class Expense : Transaction
    {
        public Expense(decimal amount) : base(amount)
        {
        }

        [JsonConstructor]
        public Expense(decimal amount, Guid guid) : base(amount, guid)
        {
        }

        public ExpenseCategory Category { get; set; }

        public override decimal Value => -base.Value;
    }
}