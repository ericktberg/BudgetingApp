using BudgetingApp.Model;
using BudgetingApp.MVVM;
using System.Collections.ObjectModel;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{
    public class ManageAccountsViewModel : ViewModelBase, IAddAccounts
    {
        public ManageAccountsViewModel(AccountManager manager)
        {
            AccountManager = manager;

            Accounts = new ObservableCollection<AccountViewModel>();
            AddAccountsObject = new AddAccountsViewModel(this, AccountManager.Accounts);

            ListUtils.MatchListChanges(Accounts, AccountManager.Accounts, acc => new AccountViewModel(acc, AccountManager), wrap => wrap.Account);
        }

        public AccountManager AccountManager { get; set; }

        public ObservableCollection<AccountViewModel> Accounts { get; }

        public AddAccountsViewModel AddAccountsObject { get; }

        public void AddAccount(Account account)
        {
            AccountManager.Accounts.Add(account);
            Accounts.Add(new AccountViewModel(account, AccountManager));
        }
    }
}