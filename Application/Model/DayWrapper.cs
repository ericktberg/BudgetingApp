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

            Statements = new ObservableCollection<Statement>(Day.Statements);
            Transactions = new ObservableCollection<Transaction>(Day.TransactionCollection);

            Day.StatementsChanged += Day_StatementsChanged;
            Day.TransactionsChanged += Day_TransactionsChanged;

            RemoveStatementCommand = new RelayCommand<Statement>(RemoveStatement);
            RemoveTransactionCommand = new RelayCommand<Transaction>(RemoveTransaction);
        }

        public ICommand RemoveStatementCommand { get; }

        public ICommand RemoveTransactionCommand { get; }

        public DateTime Date => Day.Date;

        public FinancialDay Day { get; }

        public ObservableCollection<Statement> Statements { get; }

        public ObservableCollection<Transaction> Transactions { get; }

        public void RemoveStatement(Statement statement)
        {
            Day.RemoveStatement(statement);
        }

        public void RemoveTransaction(Transaction transaction)
        {
            Day.RemoveTransaction(transaction);
        }

        private void Day_StatementsChanged(object sender, EventArgs e)
        {
            ListUtils.MatchListChanges(Statements, Day.Statements);
        }

        private void Day_TransactionsChanged(object sender, EventArgs e)
        {
            ListUtils.MatchListChanges(Transactions, Day.TransactionCollection);
        }
    }
}
