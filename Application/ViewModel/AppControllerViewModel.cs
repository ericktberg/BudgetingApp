using BudgetingApp.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingApp.ViewModel
{
    public class AppControllerViewModel : ViewModelBase
    {
        private string _currentPage = "DASHBOARD";

        public string CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

    }
}
