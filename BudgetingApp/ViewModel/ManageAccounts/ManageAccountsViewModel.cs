using BudgetingApp.Model;
using BudgetingApp.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{
    public class ManageAccountsViewModel : ViewModelBase, IAddAccounts
    {
        public ManageAccountsViewModel() : this(new AccountManager() { Accounts = { new Account("Checking", AccountType.Liquid) } })
        {
        }

        public ManageAccountsViewModel(AccountManager manager)
        {
            AccountManager = manager;

            Accounts = new ObservableCollection<AccountViewModel>();
            AddAccountsObject = new AddAccountsViewModel(this, AccountManager.Accounts);

            ListUtils.MatchListChanges(Accounts, AccountManager.Accounts, acc => new AccountViewModel(acc, AccountManager), wrap => wrap.Account);

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

        #region Command

        public ICommand AssetsCommand { get; }

        public ICommand DebtCommand { get; }

        public ICommand InvestedCommand { get; }

        public ICommand LiabilitiesCommand { get; }

        public ICommand LiquidCommand { get; }

        public ICommand NetWorthCommand { get; }

        public ICommand PropertyCommand { get; }

        public ICommand SelectAccountCommand { get; }

        #endregion Command

        public AccountManager AccountManager { get; set; }

        public ObservableCollection<AccountViewModel> Accounts { get; }

        public AddAccountsViewModel AddAccountsObject { get; }

        #region Account Lists

        public IEnumerable<AccountViewModel> Debt => Accounts.Where(a => a.Type.Equals(AccountType.Debt));

        public IEnumerable<AccountViewModel> Invested => Accounts.Where(a => a.Type.Equals(AccountType.Invested));

        public IEnumerable<AccountViewModel> Liquid => Accounts.Where(a => a.Type.Equals(AccountType.Liquid));

        public IEnumerable<AccountViewModel> Property => Accounts.Where(a => a.Type.Equals(AccountType.Property));

        public IEnumerable<AccountViewModel> SelectedAccounts => Accounts.Where(a => a.IsSelected);

        #endregion Account Lists

        public decimal NetBalance => SelectedAccounts.Sum(a => AccountManager.GetBalanceFromToday(a.Account) * (a.Type == AccountType.Debt ? -1 : 1));

        public ICommand RemoveCommand { get; }

        public void AddAccount(Account account)
        {
            AccountManager.Accounts.Add(account);
            Accounts.Add(new AccountViewModel(account, AccountManager));
            ReloadAccountLists();
        }

        public void RemoveAccount(AccountViewModel account)
        {
            AccountManager.Accounts.Remove(account.Account);
            Accounts.Remove(account);
            ReloadAccountLists();
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

        public void SelectAccount(AccountViewModel viewModel)
        {
            viewModel.IsSelected = !viewModel.IsSelected;
            OnPropertyChanged(nameof(NetBalance));
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
    }
}