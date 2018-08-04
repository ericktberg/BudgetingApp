using BudgetingApp.Model;
using BudgetingApp.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{
    public class AddAccountsViewModel : ViewModelBase
    {
        private string _accountName;
        private AccountType _type;

        public AddAccountsViewModel(IAddAccounts addAccountTo, IEnumerable<Account> accounts)
        {
            AddAccountTo = addAccountTo;
            Accounts = accounts;

            AddCommand = new RelayCommand(obj => AddAccount(), obj => !NameTaken);
        }

        #region Commands

        public RelayCommand AddCommand { get; }

        #endregion  

        public IAddAccounts AddAccountTo { get; }

        public IEnumerable<Account> Accounts { get; }


        public string AccountName
        {
            get => _accountName;
            set
            {
                _accountName = value;
                OnPropertyChanged(nameof(AccountName));
                AddCommand.OnCanExecuteChanged();
            }
        }

        public AccountType Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public bool NameTaken => Accounts.Any(c => c.Name == AccountName);

        public void AddAccount()
        {
            try
            {
                AddAccountTo.AddAccount(Create());
            }
            catch (ArgumentException)
            {
                // swallow duplicate account exception.
            }
        }

        public Account Create()
        {
            if (NameTaken) throw new ArgumentException(nameof(NameTaken));

            if (Type == AccountType.Debt)
            {
                return new DebtAccount(AccountName);
            }
            else
            {
                return new Account(AccountName, Type);
            }
        }
    }
}
