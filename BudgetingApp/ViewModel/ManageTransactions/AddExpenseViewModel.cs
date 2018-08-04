using BudgetingApp.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{
    public class AddExpenseViewModel : AddTransactionViewModel
    {
        private Account _withdrawAccount;

        public AddExpenseViewModel(Account account) : base(account)
        {
        }

        public AddExpenseViewModel(AddTransactionViewModel copyFrom) : base(copyFrom)
        {
        }
        
        public override void AddTransaction()
        {
            Account.Withdraw(new Expense(Amount), Date);
        }
    }
}
