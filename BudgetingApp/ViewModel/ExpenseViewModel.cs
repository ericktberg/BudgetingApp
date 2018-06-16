namespace BudgetingApp.ViewModel
{
    public class ExpenseViewModel : TransactionViewModel
    {
        public override string HeaderText => "Expenses";

        protected override void Cancel(object notUsed)
        {
            Create = null;
        }

        protected override void FinishCreate(object notUsed)
        {
        }

        protected override void StartCreate(object notUsed)
        {
            Create = new CreateExpenseViewModel();
        }
    }
}