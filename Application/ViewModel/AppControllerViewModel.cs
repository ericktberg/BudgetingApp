using Sunsets.Application.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Application.ViewModel
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
