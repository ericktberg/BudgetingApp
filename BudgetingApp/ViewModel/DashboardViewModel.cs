using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;

namespace BudgetingApp.ViewModel
{
    public enum TransactionType
    {
        Income,
        Expense,
        Transfer,
    }

    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel()
        {
            IncomeViewModel = new IncomeViewModel();
            ExpenseViewModel = new ExpenseViewModel();
            TransferViewModel = new TransferViewModel();
        }

        public IncomeViewModel IncomeViewModel { get; }

        public ExpenseViewModel ExpenseViewModel { get; }
        
        public TransferViewModel TransferViewModel { get; }
    }
}
