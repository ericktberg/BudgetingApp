using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Accounts
{
    public class DebtAccount : Account
    {
        public DebtAccount(string name) : base(name, AccountType.Debt)
        {
        }

        public override decimal GetDepositDelta(decimal amount)
        {
            return -base.GetDepositDelta(amount);
        }

        public override decimal GetWithdrawDelta(decimal amount)
        {
            return -base.GetWithdrawDelta(amount);
        }
    }
}
