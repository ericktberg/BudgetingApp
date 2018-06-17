using BudgetingApp.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.Model
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
        }

        public DateTime Date => Day.Date;

        public FinancialDay Day { get; }

        public ObservableCollection<Statement> Statements { get; }

        public ObservableCollection<Transaction> Transactions { get; }

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
