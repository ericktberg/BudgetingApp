using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Transactions.Accounts;

namespace Transactions
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class Transfer : Transaction
    {
        public Transfer(decimal amount, Account withdrawnFrom, Account depositedTo) 
            : base(amount, withdrawnFrom, depositedTo)
        {
        }
    }
}
