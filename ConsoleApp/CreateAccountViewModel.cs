using Sunsets.Application;
using Sunsets.Application.MVVM;
using Sunsets.Transactions;
using System.Windows.Input;

namespace Sunsets.ConsoleApp
{
    public class CreateAccountViewModel : ViewModelBase
    {
        private string _accountName;

        public CreateAccountViewModel(AccountManager manager)
        {
            Manager = manager;

            CreateAccountCommand = new RelayCommand(obj => CreateAccount(), obj => CanCreateAccount);
        }

        #region Dependencies

        private AccountManager Manager { get; }

        #endregion Dependencies

        #region Commands

        public ICommand CreateAccountCommand { get; }

        #endregion

        public string AccountName
        {
            get { return _accountName; }
            set
            {
                _accountName = value;
                OnPropertyChanged(nameof(AccountName));
                OnPropertyChanged(nameof(CanCreateAccount));
            }
        }

        private AccountType _accountType;

        public AccountType AccountType
        {
            get { return _accountType; }
            set
            {
                _accountType = value;
                OnPropertyChanged(nameof(AccountType));
            }
        }

        public bool CanCreateAccount
        {
            get => !string.IsNullOrEmpty(AccountName);
        }

        private void CreateAccount()
        {
            if (AccountType == AccountType.Liability)
            {
                Manager.AddAccount(new LiabilityAccount(AccountName));
            }
            else
            {
                Manager.AddAccount(new Account(AccountName, AccountType));
            }
        }
    }
}