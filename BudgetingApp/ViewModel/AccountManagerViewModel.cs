using BudgetingApp.Model;
using BudgetingApp.Model.FileManager;
using BudgetingApp.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Transactions;

namespace BudgetingApp.ViewModel
{

    public class AccountManagerViewModel : ViewModelBase
    {

        public AccountManagerViewModel() : this(new FileManager())
        {
        }

        public AccountManagerViewModel(IManageFiles fileManager)
        {
            FileManager = fileManager;

            AccountManager = FileManager.GetAccountManagerFromPath(FileManager.DefaultFilePath);

            Transactions = new ManageTransactionsViewModel(AccountManager);
            Accounts = new ManageAccountsViewModel(AccountManager);
            Statements = new ManageStatementsViewModel(AccountManager);

            ClosingCommand = new RelayCommand(obj => OnShutdown());
        }

        public AccountManager AccountManager { get; set; }
        
        public ManageTransactionsViewModel Transactions { get; }

        public ManageAccountsViewModel Accounts { get; }

        public ManageStatementsViewModel Statements { get; }
        
        public void OnShutdown()
        {
            FileManager.SaveAccountManagerToPath(FileManager.DefaultFilePath, AccountManager);
        }

        #region Commands

        public ICommand ClosingCommand { get; }

        #endregion Commands

        #region Dependencies

        private IManageFiles FileManager { get; }

        #endregion Dependencies
    }
}