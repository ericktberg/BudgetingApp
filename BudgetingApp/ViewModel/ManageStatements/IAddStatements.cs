using System;
using Transactions;
using Transactions.Accounts;

namespace BudgetingApp.ViewModel
{
    public interface IAddStatements
    {
        void AddStatement(Statement statement, DateTime date);
    }
}