using Sunsets.Application.Model;
using Sunsets.Application.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sunsets.Transactions;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Application.ViewModel
{
    public enum TransactionType
    {
        Income,
        Expense,
        Transfer,
    }

    public class TransactionsViewModel : ViewModelBase
    {
        public TransactionsViewModel()
        {
        }

        public TransactionsViewModel(Account account)
        {
            Account = account;
            TransactionTypes = EnumUtils.ToList<TransactionType>();
            Days = ListUtils.WrapEnumerable(DaysWithTransactions, day => new DayWrapper(day));
        }

        public Account Account { get; set; }

        public ObservableCollection<DayWrapper> Days { get; }

        public IList<TransactionType> TransactionTypes { get; }

        private IEnumerable<FinancialDay> DaysWithTransactions => Account.Calendar.Days.Where(d => d.TransactionCollection.Count > 0).Reverse();
    }
}