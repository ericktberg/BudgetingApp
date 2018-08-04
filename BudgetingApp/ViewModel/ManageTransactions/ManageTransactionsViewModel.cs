using BudgetingApp.Model;
using BudgetingApp.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

    public class ManageTransactionsViewModel : ViewModelBase
    {
        private AddTransactionViewModel _addTransactions;

        private TransactionType _selectedType;



        public ManageTransactionsViewModel()
        {
            TransactionType = TransactionType.Income;
            TransactionTypes = EnumUtils.ToList<TransactionType>();
            Days = ListUtils.WrapEnumerable(DaysWithTransactions, day => new DayWrapper(day));

            Account = new Account("Account", AccountType.Liquid);
            Account.Deposit(new Income(500), new DateTime(2000, 1, 1));
            Account.Deposit(new Income(500), new DateTime(2000, 1, 1));
            Account.Deposit(new Income(500), new DateTime(2000, 1, 1));
            Account.Deposit(new Income(500), new DateTime(2000, 1, 1));
            Account.Deposit(new Income(500), new DateTime(2000, 1, 1));
        }

        public ManageTransactionsViewModel(Account account)
        {
            Account = account;

            TransactionType = TransactionType.Income;
            TransactionTypes = EnumUtils.ToList<TransactionType>();
            Days = ListUtils.WrapEnumerable(DaysWithTransactions, day => new DayWrapper(day));
        }

        public Account Account { get; set; }

        public AddTransactionViewModel AddTransactionsObject
        {
            get => _addTransactions;
            set
            {
                _addTransactions = value;
                OnPropertyChanged(nameof(AddTransactionsObject));
            }
        }

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

        public ObservableCollection<DayWrapper> Days { get; }

        public IList<TransactionType> TransactionTypes { get; }
        
        public void GetModelTypeFromEnum(TransactionType type)
        {
            if (AddTransactionsObject == null)
            {
                switch (type)
                {
                    case TransactionType.Income:
                        AddTransactionsObject = new AddIncomeViewModel(Account);
                        return;

                    case TransactionType.Expense:
                        AddTransactionsObject = new AddExpenseViewModel(Account);
                        return;

                    case TransactionType.Transfer:
                        AddTransactionsObject = new AddTransferViewModel(Account, Accounts);
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
                            AddTransactionsObject = new AddTransferViewModel(AddTransactionsObject, Accounts);
                        }
                        return;
                }
            }
        }


        private IEnumerable<FinancialDay> DaysWithTransactions => Account.Calendar.Days.Where(d => d.TransactionCollection.Count > 0).Reverse();

        private void MatchDays()
        {
            ListUtils.MatchListChanges(Days, DaysWithTransactions, day => new DayWrapper(day), wrap => wrap.Day);
        }
    }
}