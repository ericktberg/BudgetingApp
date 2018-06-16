using System;
using System.Configuration;
using Transactions.Accounts;

namespace Transactions
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
        public Expense(decimal amount, Account withdrawnFrom) : base(amount, withdrawnFrom, null)
        {
        }
        
        public ExpenseCategory Category { get; set; }
    }
}