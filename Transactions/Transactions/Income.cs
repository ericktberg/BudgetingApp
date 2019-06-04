using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Transactions
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
        public Income(decimal amount) : base(amount)
        {
        }

        [JsonConstructor]
        public Income(decimal amount, Guid guid) : base(amount, guid)
        {
        }

        public IncomeCategory Category { get; set; }

        public override decimal Value => base.Value;
    }
}
