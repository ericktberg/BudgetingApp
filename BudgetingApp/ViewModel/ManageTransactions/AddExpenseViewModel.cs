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

        public AddExpenseViewModel(IAddTransactions addTo, IEnumerable<Account> accounts) : base(addTo, accounts)
        {
        }

        public AddExpenseViewModel(AddTransactionViewModel copyFrom) : base(copyFrom)
        {
        }

        public Account WithdrawAccount
        {
            get => _withdrawAccount;
            set
            {
                _withdrawAccount = value;
                OnPropertyChanged(nameof(WithdrawAccount));
            }
        }

        public override bool AccountValid => WithdrawAccount != null;

        protected override Transaction InnerCreate()
        {
            if (!AccountValid) throw new ArgumentException(nameof(WithdrawAccount));

            return new Expense(Amount, WithdrawAccount);
        }
    }
}
