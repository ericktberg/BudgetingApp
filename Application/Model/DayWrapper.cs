using Sunsets.Application.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Sunsets.Transactions;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Application.Model
{
    public class DayWrapper : ViewModelBase
    {
        public DayWrapper(FinancialDay day)
        {
            Day = day;

            Statements = Day.Statement;
            Transactions = new ObservableCollection<ITransaction>(Day.TransactionCollection);
            
            Day.TransactionsChanged += Day_TransactionsChanged;

            RemoveStatementCommand = new RelayCommand<Statement>(RemoveStatement);
            RemoveTransactionCommand = new RelayCommand<ITransaction>(RemoveTransaction);
        }

        public ICommand RemoveStatementCommand { get; }

        public ICommand RemoveTransactionCommand { get; }

        public DateTime Date => Day.Date;

        public FinancialDay Day { get; }

        public Statement Statements { get; set; }

        public ObservableCollection<ITransaction> Transactions { get; }

        public void RemoveStatement(Statement statement)
        {
            Statements = null;
        }

        public void RemoveTransaction(ITransaction transaction)
        {
            Day.RemoveTransaction(transaction);
        }
        
        private void Day_TransactionsChanged(object sender, EventArgs e)
        {
            ListUtils.MatchListChanges(Transactions, Day.TransactionCollection);
        }
    }
}
