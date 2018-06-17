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

        public AddTransactionViewModel(IAddTransactions addTo, IEnumerable<Account> accounts)
        {
            AddTo = addTo;
            Accounts = accounts;
            Date = DateTime.Now;

            AddCommand = new RelayCommand(obj => AddTransaction(), obj => DateValid && AccountValid);
        }

        public AddTransactionViewModel(AddTransactionViewModel copyFrom) : this(copyFrom.AddTo, copyFrom.Accounts)
        {
            Amount = copyFrom.Amount;
            Date = copyFrom.Date;
        }

        #region Commands

        public RelayCommand AddCommand { get; }

        #endregion

        public IAddTransactions AddTo { get; }

        public IEnumerable<Account> Accounts { get; }

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

        public void AddTransaction()
        {
            AddTo.AddTransaction(Create(), Date);
        }

        public abstract bool AccountValid {get;}


        public Transaction Create()
        {
            if (!DateValid) throw new ArgumentException(nameof(Date));

            return InnerCreate();
        }

        protected abstract Transaction InnerCreate();
    }
}
