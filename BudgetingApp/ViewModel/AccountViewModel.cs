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
    public class AccountViewModel : ViewModelBase
    {
        private bool _isSelected;

        public AccountViewModel(Account account, AccountManager manager)
        {
            Account = account;
            Manager = manager;
        }

        public AccountManager Manager { get; }

        public Account Account { get; }

        public string Name => Account.Name;

        public AccountType Type => Account.Type;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public decimal CurrentBalance => Manager.GetBalanceFromToday(Account);
    }
}
