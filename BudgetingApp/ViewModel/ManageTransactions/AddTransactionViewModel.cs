using BudgetingApp.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{


    public abstract class AddTransactionViewModel : ViewModelBase
    {
        private decimal _amount;
        private DateTime _date;

        public AddTransactionViewModel(Account account)
        {
            Date = DateTime.Now;

            AddCommand = new RelayCommand(obj => AddTransaction(), obj => DateValid);
        }

        public AddTransactionViewModel(AddTransactionViewModel copyFrom) : this(copyFrom.Account)
        {
            Amount = copyFrom.Amount;
            Date = copyFrom.Date;
        }

        #region Commands

        public RelayCommand AddCommand { get; }

        #endregion

        public Account Account { get; }

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

        public bool DateValid
        {
            get => Date.Year > 1995;
        }

        public abstract void AddTransaction();
    }
}
