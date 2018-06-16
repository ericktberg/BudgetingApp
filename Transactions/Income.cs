using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Transactions.Accounts;

namespace Transactions
{
    public enum IncomeCategory
    {
        Paycheck,
        Venmo,
        Cash
    }

    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class Income : Transaction
    {
        public Income(decimal amount, Account depositedTo) : base(amount, null, depositedTo)
        {
        }
        
        public IncomeCategory Category { get; set; }
        
    }
}
