using BudgetingApp.Model;
using BudgetingApp.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Transactions;

namespace BudgetingApp.ViewModel
{
    public enum TransactionType
    {
        Income,
        Expense,
        Transfer,
    }

    public class ManageTransactionsViewModel : ViewModelBase, IAddTransactions
    {
        private AddTransactionViewModel _addTransactions;

        private TransactionType _selectedType;

        public ManageTransactionsViewModel(AccountManager manager)
        {
            AccountManager = manager;

            TransactionType = TransactionType.Income;
            TransactionTypes = EnumsUtil.ToList<TransactionType>();
            Days = ListUtils.WrapEnumerable(DaysWithTransactions, day => new DayWrapper(day));
        }

        public AccountManager AccountManager { get; set; }

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

        public void AddTransaction(Transaction transaction, DateTime date)
        {
            AccountManager.AddTransaction(transaction, date);
            MatchDays();
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


        private IEnumerable<FinancialDay> DaysWithTransactions => AccountManager.Calendar.Days.Where(d => d.TransactionCollection.Count > 0).Reverse();

        private void MatchDays()
        {
            ListUtils.MatchListChanges(Days, DaysWithTransactions, day => new DayWrapper(day), wrap => wrap.Day);
        }
    }
}