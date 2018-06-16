using BudgetingApp.ViewModel.Create;

namespace BudgetingApp.ViewModel
{
    public class TransferViewModel : TransactionViewModel
    {
        public override string HeaderText => "Transfers";

        protected override void Cancel(object notUsed)
        {
            Create = null;
        }

        protected override void FinishCreate(object notUsed)
        {
        }

        protected override void StartCreate(object notUsed)
        {
            Create = new CreateTransferViewModel();
        }
    }
}