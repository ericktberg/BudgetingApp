using System;
using Transactions;

namespace BudgetingApp.ViewModel
{
    public interface IAddTransactions
    {
        void AddTransaction(Transaction transaction, DateTime date);
    }
}