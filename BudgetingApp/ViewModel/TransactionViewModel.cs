using System.Collections.ObjectModel;
using Transactions;

namespace BudgetingApp.ViewModel
{
    public abstract class TransactionViewModel : ViewModelBase
    {
        private CreateTransactionViewModel _create = null;

        public TransactionViewModel()
        {
            CancelCommand = new RelayCommand<object>(Cancel);
            FinishCommand = new RelayCommand<object>(FinishCreate);
            StartCreateCommand = new RelayCommand<object>(StartCreate);
        }

        public RelayCommand<object> CancelCommand { get; }

        public ObservableCollection<Transaction> Collection { get; } = new ObservableCollection<Transaction>();

        public CreateTransactionViewModel Create
        {
            get => _create;
            set
            {
                _create = value;
                OnPropertyChanged(nameof(Create));
            }
        }

        public RelayCommand<object> FinishCommand { get; }

        public abstract string HeaderText { get; }

        public RelayCommand<object> StartCreateCommand { get; }

        protected abstract void Cancel(object notUsed);

        protected abstract void FinishCreate(object notUsed);

        protected abstract void StartCreate(object notUsed);
    }
}