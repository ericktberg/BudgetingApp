using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{
    public enum TransferMode
    {
        From,
        To
    }

    public class AddTransferViewModel : AddTransactionViewModel
    {
        private Account _otherAccount;
        private TransferMode _transferMode;

        public AddTransferViewModel(AddTransactionViewModel copyFrom, IEnumerable<Account> accounts) : base(copyFrom)
        {
            Accounts = accounts;
        }

        public AddTransferViewModel(Account account, IEnumerable<Account> accounts) : base(account)
        {
            Accounts = accounts;
        }

        public IEnumerable<Account> Accounts { get; }
        
        public Account OtherAccount
        {
            get => _otherAccount;
            set
            {
                _otherAccount = value;
                OnPropertyChanged(nameof(OtherAccount));
            }
        }

        public TransferMode TransferMode
        {
            get => _transferMode;
            set
            {
                _transferMode = value;
                OnPropertyChanged(nameof(TransferMode));
            }
        }

        public bool AccountValid => OtherAccount != null && !OtherAccount.Equals(Account);

        public override void AddTransaction()
        {
            if (!AccountValid) throw new ArgumentException("Other account invalid or the same as current account.");

            switch (TransferMode)
            {
                case TransferMode.From:
                    Account.TransferFrom(new TransferFrom(Amount, OtherAccount), Date);
                    break;

                case TransferMode.To:
                    Account.TransferTo(new TransferTo(Amount, OtherAccount), Date);
                    break;
            }
        }
    }
}
