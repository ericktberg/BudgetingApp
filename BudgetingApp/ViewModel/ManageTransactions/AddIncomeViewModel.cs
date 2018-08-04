using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{
    public class AddIncomeViewModel : AddTransactionViewModel
    {
        private Account _depositAccount;

        public AddIncomeViewModel(AddTransactionViewModel copyFrom) : base(copyFrom)
        {
        }

        public AddIncomeViewModel(Account account) : base(account)
        {
        }
        
        public override void AddTransaction()
        {
            Account.Deposit(new Income(Amount), Date);
        }
    }
}
