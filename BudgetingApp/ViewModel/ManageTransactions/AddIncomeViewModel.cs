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

        public AddIncomeViewModel(IAddTransactions addTo, IEnumerable<Account> accounts) : base(addTo, accounts)
        {
        }

        public Account DepositAccount
        {
            get => _depositAccount;
            set
            {
                _depositAccount = value;
                OnPropertyChanged(nameof(DepositAccount));
            }
        }

        public override bool AccountValid => DepositAccount != null;
        
        protected override Transaction InnerCreate()
        {
            if (!AccountValid) throw new ArgumentException(nameof(DepositAccount));

            return new Income(Amount, DepositAccount);
        }
    }
}
