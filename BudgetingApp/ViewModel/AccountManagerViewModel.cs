using BudgetingApp.Model;
using BudgetingApp.Model.FileManager;
using BudgetingApp.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{
    public enum TransactionType
    {
        Income,
        Expense,
        Transfer,
    }

    public interface IAddStatements
    {
        void AddStatement(Statement statement, DateTime date);
    }

    public interface IAddTransactions
    {
        void AddTransaction(Transaction transaction, DateTime date);
    }

    public class AccountManagerViewModel : ViewModelBase, IAddAccounts, IAddStatements, IAddTransactions
    {
        private AddTransactionViewModel _addTransactions;
        private TransactionType _selectedType;

        public AccountManagerViewModel() : this(new FileManager())
        {
        }

        public AccountManagerViewModel(IManageFiles fileManager)
        {
            FileManager = fileManager;

            AccountManager = FileManager.GetAccountManagerFromPath(FileManager.DefaultFilePath);
            Accounts = new ObservableCollection<AccountViewModel>();
            ListUtils.MatchListChanges(Accounts, AccountManager.Accounts, acc => new AccountViewModel(acc, AccountManager), wrap => wrap.Account);

            AddAccountsObject = new AddAccountsViewModel(this, AccountManager.Accounts);
            AddStatementsObject = new AddStatementsViewModel(this, AccountManager.Accounts);

            TransactionType = TransactionType.Income;
            TransactionTypes = EnumsUtil.ToList<TransactionType>();

            Days = new ObservableCollection<DayWrapper>();
            ListUtils.MatchListChanges(Days, AccountManager.Calendar.Days, day => new DayWrapper(day), wrap => wrap.Day);

            AccountManager.Calendar.DayCollectionChanged += Calendar_DayCollectionChanged;

            ClosingCommand = new RelayCommand(obj => OnShutdown());
        }

        public AccountManager AccountManager { get; set; }

        public ObservableCollection<AccountViewModel> Accounts { get; }

        public AddAccountsViewModel AddAccountsObject { get; }

        public AddStatementsViewModel AddStatementsObject { get; }

        public AddTransactionViewModel AddTransactionsObject
        {
            get => _addTransactions;
            set
            {
                _addTransactions = value;
                OnPropertyChanged(nameof(AddTransactionsObject));
            }
        }

        public ObservableCollection<DayWrapper> Days { get; }

        public TransactionType TransactionType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged(nameof(TransactionType));

                GetModelTypeFromEnum(value);
            }
        }

        public void GetModelTypeFromEnum(TransactionType type)
        {
            if (AddTransactionsObject == null)
            {
                switch (type)
                {
                    case TransactionType.Income:
                        AddTransactionsObject = new AddIncomeViewModel(this, AccountManager.Accounts);
                        return;

                    case TransactionType.Expense:
                        AddTransactionsObject = new AddExpenseViewModel(this, AccountManager.Accounts);
                        return;

                    case TransactionType.Transfer:
                        AddTransactionsObject = new AddTransferViewModel(this, AccountManager.Accounts);
                        return;
                }
            }
            else
            {
                switch (type)
                {
                    case TransactionType.Income:
                        if (!(AddTransactionsObject is AddIncomeViewModel))
                        {
                            AddTransactionsObject = new AddIncomeViewModel(AddTransactionsObject);
                        }
                        return;

                    case TransactionType.Expense:
                        if (!(AddTransactionsObject is AddExpenseViewModel))
                        {
                            AddTransactionsObject = new AddExpenseViewModel(AddTransactionsObject);
                        }
                        return;

                    case TransactionType.Transfer:
                        if (!(AddTransactionsObject is AddTransferViewModel))
                        {
                            AddTransactionsObject = new AddTransferViewModel(AddTransactionsObject);
                        }
                        return;
                }
            }
        }

        public IList<TransactionType> TransactionTypes { get; }

        public void AddAccount(Account account)
        {
            AccountManager.Accounts.Add(account);
            Accounts.Add(new AccountViewModel(account, AccountManager));
        }

        public void AddStatement(Statement statement, DateTime date)
        {
            AccountManager.AddStatement(statement, date);
        }

        public void AddTransaction(Transaction transaction, DateTime date)
        {
            AccountManager.AddTransaction(transaction, date);
        }

        public void OnShutdown()
        {
            FileManager.SaveAccountManagerToPath(FileManager.DefaultFilePath, AccountManager);
        }

        private void Calendar_DayCollectionChanged(object sender, EventArgs e)
        {
            ListUtils.MatchListChanges(Days, AccountManager.Calendar.Days, day => new DayWrapper(day), wrap => wrap.Day);
        }

        #region Commands

        public ICommand ClosingCommand { get; }

        #endregion Commands

        #region Dependencies

        private IManageFiles FileManager { get; }

        #endregion Dependencies
    }
}