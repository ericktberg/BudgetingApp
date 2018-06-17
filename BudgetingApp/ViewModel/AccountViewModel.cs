using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{
    public class AccountViewModel
    {
        public AccountViewModel(Account account, AccountManager manager)
        {
            Account = account;
            Manager = manager;
        }

        public AccountManager Manager { get; }

        public Account Account { get; }

        public string Name => Account.Name;

        public AccountType Type => Account.Type;

        public decimal CurrentBalance => Manager.GetBalanceFromToday(Account);
    }
}
