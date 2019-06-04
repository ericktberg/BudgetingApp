using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions.Accounts
{
    public class DebtAccount : Account
    {
        public DebtAccount(string name) : base(name, AccountType.Debt)
        {
        }

        public override decimal GetDelta(decimal amount)
        {
            return -base.GetDelta(amount);
        }
    }
}
