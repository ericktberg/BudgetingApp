using Sunsets.Application.Model;
using Sunsets.Application.Model.FileManager;
using Sunsets.Application.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Sunsets.Transactions;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Application.ViewModel
{
    public class AccountManagerViewModel : ViewModelBase
    {
        private AccountViewModel _selectedAccount;

        public AccountManagerViewModel()
        {
            AccountManager = new AccountManager();

            Accounts = new ObservableCollection<AccountViewModel>();

            AddAccount(new Account("Pizza", AccountType.Liquid));
            AddAccount(new Account("Unicorns", AccountType.Debt));
            AddAccount(new Account("Robot", AccountType.Property));
            AddAccount(new Account("MasterChief", AccountType.Invested));

            SelectedAccount = Accounts[1];
        }

        public AccountManagerViewModel(IManageFiles fileManager)
        {
            FileManager = fileManager;

            AccountManager = FileManager.GetAccountManagerFromPath(FileManager.DefaultFilePath);

            ClosingCommand = new RelayCommand(obj => OnShutdown());

            Accounts = new ObservableCollection<AccountViewModel>();
            ListUtils.MatchListChanges(Accounts, AccountManager.Accounts, acc => new AccountViewModel(acc), wrap => wrap.Account);

            RemoveCommand = new RelayCommand<AccountViewModel>(RemoveAccount);
            AssetsCommand = new RelayCommand(obj => SelectAssets());
            DebtCommand = new RelayCommand(obj => SelectDebt());
            InvestedCommand = new RelayCommand(obj => SelectInvested());
            LiabilitiesCommand = new RelayCommand(obj => SelectLiabilities());
            LiquidCommand = new RelayCommand(obj => SelectLiquid());
            PropertyCommand = new RelayCommand(obj => SelectProperty());
            NetWorthCommand = new RelayCommand(obj => SelectNetWorth());
            SelectAccountCommand = new RelayCommand<AccountViewModel>(SelectAccount);
        }

        public AccountManager AccountManager { get; set; }

        public ObservableCollection<AccountViewModel> Accounts { get; }

        public AccountViewModel SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }

        public decimal NetBalance => SelectedAccounts.Sum(a => AccountManager.GetBalanceFromToday(a.Account) * (a.Type == AccountType.Debt ? -1 : 1));

        public ICommand RemoveCommand { get; }

        public void AddAccount(Account account)
        {
            AccountManager.Accounts.Add(account);
            Accounts.Add(new AccountViewModel(account));
            ReloadAccountLists();
        }

        public void OnShutdown()
        {
            FileManager.SaveAccountManagerToPath(FileManager.DefaultFilePath, AccountManager);
        }

        public void RemoveAccount(AccountViewModel account)
        {
            AccountManager.Accounts.Remove(account.Account);
            Accounts.Remove(account);
            ReloadAccountLists();
        }

        public void SelectAccount(AccountViewModel viewModel)
        {
            viewModel.IsSelected = !viewModel.IsSelected;
            OnPropertyChanged(nameof(NetBalance));
        }

        public void SelectAssets()
        {
            SelectType(new List<AccountType> { AccountType.Liquid, AccountType.Invested, AccountType.Property });
        }

        public void SelectDebt()
        {
            SelectType(AccountType.Debt);
        }

        public void SelectInvested()
        {
            SelectType(AccountType.Invested);
        }

        public void SelectLiabilities()
        {
            SelectType(AccountType.Debt);
        }

        public void SelectLiquid()
        {
            SelectType(AccountType.Liquid);
        }

        public void SelectNetWorth()
        {
            SelectType();
        }

        public void SelectProperty()
        {
            SelectType(AccountType.Property);
        }

        private void ReloadAccountLists()
        {
            OnPropertyChanged(nameof(Liquid));
            OnPropertyChanged(nameof(Invested));
            OnPropertyChanged(nameof(Property));
            OnPropertyChanged(nameof(Debt));
        }

        private void SelectType()
        {
            SelectType(EnumUtils.ToList<AccountType>());
        }

        private void SelectType(AccountType typeToSelect)
        {
            SelectType(new List<AccountType> { typeToSelect });
        }

        private void SelectType(IEnumerable<AccountType> accountsToSelect)
        {
            foreach (var a in Accounts)
            {
                a.IsSelected = accountsToSelect.Contains(a.Type);
            }
            OnPropertyChanged(nameof(NetBalance));
        }

        #region Account Lists

        public IEnumerable<AccountViewModel> Debt => Accounts.Where(a => a.Type.Equals(AccountType.Debt));

        public IEnumerable<AccountViewModel> Invested => Accounts.Where(a => a.Type.Equals(AccountType.Invested));

        public IEnumerable<AccountViewModel> Liquid => Accounts.Where(a => a.Type.Equals(AccountType.Liquid));

        public IEnumerable<AccountViewModel> Property => Accounts.Where(a => a.Type.Equals(AccountType.Property));

        public IEnumerable<AccountViewModel> SelectedAccounts => Accounts.Where(a => a.IsSelected);

        #endregion Account Lists

        #region Commands

        public ICommand AssetsCommand { get; }

        public ICommand ClosingCommand { get; }

        public ICommand DebtCommand { get; }

        public ICommand InvestedCommand { get; }

        public ICommand LiabilitiesCommand { get; }

        public ICommand LiquidCommand { get; }

        public ICommand NetWorthCommand { get; }

        public ICommand PropertyCommand { get; }

        public ICommand SelectAccountCommand { get; }

        #endregion Commands

        #region Dependencies

        private IManageFiles FileManager { get; }

        #endregion Dependencies
    }
}