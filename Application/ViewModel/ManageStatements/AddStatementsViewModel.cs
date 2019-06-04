using BudgetingApp.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{
    public class AddStatementsViewModel : ViewModelBase
    {
        private decimal _balance;
        private Account _addToAccount;
        private DateTime _date;

        public AddStatementsViewModel(IAddStatements addStatementTo)
        {
            AddStatementTo = addStatementTo;
            Date = DateTime.Now;

            AddCommand = new RelayCommand(obj => AddStatement(), obj => DateValid && AccountValid);
        }

        #region Commands

        public RelayCommand AddCommand { get; }

        #endregion

        public IAddStatements AddStatementTo { get; }

        public IEnumerable<Account> Accounts { get; }

        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }

        public Account AddToAccount
        {
            get => _addToAccount;
            set
            {
                _addToAccount = value;
                OnPropertyChanged(nameof(AddToAccount));
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

        public bool AccountValid => AddToAccount != null;

        public void AddStatement()
        {
            AddStatementTo.AddStatement(Create(), Date);
        }

        public Statement Create()
        {
            if (!DateValid) throw new ArgumentException(nameof(Date));
            if (!AccountValid) throw new ArgumentException(nameof(AddToAccount));

            return new Statement(Balance);
        }
    }
}
