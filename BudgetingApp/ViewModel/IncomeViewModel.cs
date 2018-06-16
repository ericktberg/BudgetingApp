using System;
using Transactions;

namespace BudgetingApp.ViewModel
{
    public class IncomeViewModel : TransactionViewModel
    {
        public IncomeViewModel()
        {
        }

        public override string HeaderText => "Income";

        protected override void Cancel(object notUsed)
        {
            Create = null;
        }

        protected override void FinishCreate(object notUsed)
        {
        }

        protected override void StartCreate(object notUsed)
        {
            Create = new CreateIncomeViewModel();
        }
    }
}