using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;
namespace BudgetingApp.ViewModel
{
    public abstract class CreateTransactionViewModel  : ViewModelBase
    {
        private decimal _amount;
        private DateTime _date;
        private Transaction _transaction = null;

        public CreateTransactionViewModel()
        {
        }
        
        public ExpenseCategory Category { get; }

        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public Transaction Transaction
        {
            get => _transaction;
            set
            {
                _transaction = value;
                OnPropertyChanged(nameof(Transaction));
            }
        }
    }
}
