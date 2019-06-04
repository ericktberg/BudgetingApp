using Sunsets.Application.Model;
using Sunsets.Application.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunsets.Transactions;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Application.ViewModel
{
    public class AccountViewModel : ViewModelBase
    {
        private bool _isSelected;
        private bool _showTransactions;

        public AccountViewModel()
        {
            Account = new Account("Test Account", AccountType.Liquid);
            Account.Deposit(new Income(1000), new DateTime(2000, 1, 1));
            Account.Withdraw(new Expense(500), new DateTime(2000, 2, 1));
            Account.AddStatement(new Statement(2300), new DateTime(1999, 1, 1));

            AccountTypes = EnumUtils.ToList<AccountType>();
        }


        public AccountViewModel(Account account)
        {
            Account = account;

            AccountTypes = EnumUtils.ToList<AccountType>();
        }

        public Account Account { get; }

        public string Name
        {
            get => Account.Name;
            set
            {
                Account.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public AccountType Type
        {
            get => Account.Type;
            set
            {
                Account.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public bool ShowTransactions
        {
            get => _showTransactions;
            set
            {
                _showTransactions = value;
                OnPropertyChanged(nameof(ShowTransactions));
            }
        }

        public IEnumerable<AccountType> AccountTypes { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public decimal CurrentBalance => Account.GetBalanceFromToday();
    }
}
