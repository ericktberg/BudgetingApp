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
    public class ManageStatementsViewModel : ViewModelBase, IAddStatements
    {
        public ManageStatementsViewModel(AccountManager manager)
        {
            AccountManager = manager;

            AddStatementsObject = new AddStatementsViewModel(this, AccountManager.Accounts);

            Days = ListUtils.WrapEnumerable(DaysWithStatements, day => new DayWrapper(day));
        }
        public AccountManager AccountManager { get; }

        public AddStatementsViewModel AddStatementsObject { get; }

        public ObservableCollection<DayWrapper> Days { get; }

        public void AddStatement(Statement statement, DateTime date)
        {
            AccountManager.AddStatement(statement, date);
            MatchDays();
        }

        private IEnumerable<FinancialDay> DaysWithStatements => AccountManager.Calendar.Days.Where(d => d.Statements.Count > 0).Reverse();

        private void MatchDays()
        {
            ListUtils.MatchListChanges(Days, DaysWithStatements, day => new DayWrapper(day), wrap => wrap.Day);
        }
    }
}