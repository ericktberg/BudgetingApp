using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{
    public class AddTransferViewModel : AddTransactionViewModel
    {
        private Account _depositAccount;
        private Account _withdrawAccount;

        public AddTransferViewModel(AddTransactionViewModel copyFrom) : base(copyFrom)
        {
        }

        public AddTransferViewModel(IAddTransactions addTo, IEnumerable<Account> accounts) : base(addTo, accounts)
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

        public Account DepositAccount
        {
            get => _depositAccount;
            set
            {
                _depositAccount = value;
                OnPropertyChanged(nameof(DepositAccount));
            }
        }

        public override bool AccountValid => DepositAccount != null && WithdrawAccount != null && !DepositAccount.Equals(WithdrawAccount);

        protected override Transaction InnerCreate()
        {
            if (!AccountValid) throw new ArgumentException("Deposit account or Withdraw account invalid");

            return new Transfer(Amount, WithdrawAccount, DepositAccount);
        }
    }
}
