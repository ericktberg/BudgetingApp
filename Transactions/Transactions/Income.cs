using Newtonsoft.Json;
using System;
using System.Configuration;

namespace Sunsets.Transactions
{
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