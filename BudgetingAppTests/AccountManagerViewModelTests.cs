using System;
using System.Linq;
using BudgetingApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Transactions;
using Transactions.Accounts;

namespace BudgetingAppTests
{
    [TestClass]
    public class ManageTransactionsViewModelTests
    {
        ManageTransactionsViewModel ViewModel { get; set; } = new ManageTransactionsViewModel(new AccountManager());

        [TestClass]
        public class TransactionTypeTests : ManageTransactionsViewModelTests
        {
            [TestMethod]
            public void Should_Change_TransactionViewModel_With_TransactionType()
            {
                ViewModel.TransactionType = TransactionType.Expense;

                Assert.IsInstanceOfType(ViewModel.AddTransactionsObject, typeof(AddExpenseViewModel));

                ViewModel.TransactionType = TransactionType.Income;

                Assert.IsInstanceOfType(ViewModel.AddTransactionsObject, typeof(AddIncomeViewModel));

                ViewModel.TransactionType = TransactionType.Transfer;

                Assert.IsInstanceOfType(ViewModel.AddTransactionsObject, typeof(AddTransferViewModel));
            }

            [TestMethod]
            public void Should_Not_Change_At_All_If_Already_Of_Type()
            {
                ViewModel.TransactionType = TransactionType.Expense;
                var oldModel = ViewModel.AddTransactionsObject;

                ViewModel.TransactionType = TransactionType.Expense;
                Assert.IsTrue(ReferenceEquals(oldModel, ViewModel.AddTransactionsObject));
            }

            [TestMethod]
            public void Should_Transfer_Values_If_Changing_Type()
            {
                ViewModel.TransactionType = TransactionType.Expense;
                ViewModel.AddTransactionsObject.Amount = 500;
                ViewModel.AddTransactionsObject.Date = new DateTime(2000, 1, 1);

                ViewModel.TransactionType = TransactionType.Income;

                Assert.AreEqual(500, ViewModel.AddTransactionsObject.Amount);
                Assert.AreEqual(new DateTime(2000, 1, 1), ViewModel.AddTransactionsObject.Date);
            }
        }

        [TestClass]
        public class DaysCollection : ManageTransactionsViewModelTests
        {
            [TestMethod]
            public void Should_Have_No_Stateless_Days_On_Init()
            {
                var manager = new AccountManager()
                {
                    Accounts = { new Account("TestAccount") }
                };

                manager.Calendar.GetDayForDate(new DateTime(2000, 1, 1));

                ViewModel = new ManageTransactionsViewModel(manager);

                Assert.AreEqual(1, ViewModel.AccountManager.Calendar.Days.Count());
                Assert.AreEqual(0, ViewModel.Days.Count());
            }

            [TestMethod]
            public void Should_Have_Days_With_Statements_On_Init()
            {
                var manager = new AccountManager()
                {
                    Accounts = { new Account("TestAccount") }
                };

                manager.AddTransaction(new Income(1000, new Account("TestAccount")), new DateTime(2000, 1, 1));

                ViewModel = new ManageTransactionsViewModel(manager);

                Assert.AreEqual(1, ViewModel.AccountManager.Calendar.Days.Count());
                Assert.AreEqual(1, ViewModel.Days.Count());
            }

            [TestMethod]
            public void Should_Ignore_Days_Without_Statements_In_Them_When_Updating()
            {
                ViewModel.AccountManager.Calendar.GetDayForDate(new DateTime(2000, 1, 1));
                ViewModel.AddTransaction(new Income(1000, new Account("TestAccount")), new DateTime(2001, 2, 3));
                
                Assert.AreEqual(2, ViewModel.AccountManager.Calendar.Days.Count());
                Assert.AreEqual(1, ViewModel.Days.Count());
            }
        }
    }

    [TestClass]
    public class ManageAccountsViewModelTests
    {
        ManageAccountsViewModel ViewModel { get; } = new ManageAccountsViewModel(new AccountManager());

        [TestClass]
        public class AddAccount : ManageAccountsViewModelTests
        {
            [TestMethod]
            public void Should_Keep_Lists_Synchronized()
            {
                int beforeCount = ViewModel.AccountManager.Accounts.Count;

                Assert.AreEqual(beforeCount, ViewModel.Accounts.Count);

                ViewModel.AddAccount(new Account("TestAccount"));

                int afterCount = ViewModel.AccountManager.Accounts.Count;

                Assert.AreEqual(beforeCount + 1, afterCount);
                Assert.AreEqual(afterCount, ViewModel.Accounts.Count);
            }
        }
    }

    [TestClass]
    public class ManageStatementsViewModelTests
    {
        ManageStatementsViewModel ViewModel { get; set; } = new ManageStatementsViewModel(new AccountManager() { Accounts = { new Account("TestAccount") } });
        
        [TestClass]
        public class AddStatement : ManageStatementsViewModelTests
        {
            [TestMethod]
            public void Should_Keep_Days_Synchronized_When_Adding_Statement()
            {
                Assert.AreEqual(0, ViewModel.AccountManager.Calendar.Days.Count());
                Assert.AreEqual(0, ViewModel.Days.Count);
                var account = new Account("TestAccount");

                ViewModel.AddStatement(new Statement(1000, account), new DateTime(2000, 1, 1));

                Assert.AreEqual(1, ViewModel.AccountManager.Calendar.Days.Count());
                Assert.AreEqual(1, ViewModel.Days.Count);
            }

        }

        [TestClass]
        public class DaysCollection : ManageStatementsViewModelTests
        {
            [TestMethod]
            public void Should_Have_No_Stateless_Days_On_Init()
            {
                var manager = new AccountManager()
                {
                    Accounts = { new Account("TestAccount") }
                };

                manager.Calendar.GetDayForDate(new DateTime(2000, 1, 1));

                ViewModel = new ManageStatementsViewModel(manager);
                
                Assert.AreEqual(1, ViewModel.AccountManager.Calendar.Days.Count());
                Assert.AreEqual(0, ViewModel.Days.Count());
            }

            [TestMethod]
            public void Should_Have_Days_With_Statements_On_Init()
            {
                var manager = new AccountManager()
                {
                    Accounts = { new Account("TestAccount") }
                };

                manager.AddStatement(new Statement(1000, new Account("TestAccount")), new DateTime(2000, 1, 1));

                ViewModel = new ManageStatementsViewModel(manager);

                Assert.AreEqual(1, ViewModel.AccountManager.Calendar.Days.Count());
                Assert.AreEqual(1, ViewModel.Days.Count());
            }

            [TestMethod]
            public void Should_Ignore_Days_Without_Statements_In_Them_When_Updating()
            {
                ViewModel.AccountManager.Calendar.GetDayForDate(new DateTime(2000, 1, 1));
                ViewModel.AddStatement(new Statement(1000, new Account("TestAccount")), new DateTime(2001, 2, 3));

                Assert.AreEqual(2, ViewModel.AccountManager.Calendar.Days.Count());
                Assert.AreEqual(1, ViewModel.Days.Count());
            }
        }
    }



    [TestClass]
    public class AccountManagerViewModelTests
    {
        public static MockFileManager Files { get; } = new MockFileManager();

        public AccountManagerViewModel ViewModel { get; } = new AccountManagerViewModel(Files);

        [TestClass]
        public class Constructor : AccountManagerViewModelTests
        {
            [TestMethod]
            public void Should_Never_Have_Null_Manager_On_Load()
            {
                Assert.IsNotNull(ViewModel.AccountManager);
            }

            [TestMethod]
            public void Should_Save_Manager_On_Shutdown()
            {
                ViewModel.OnShutdown();

                Assert.IsTrue(Files.IsSaved);
                // FileManager.Verify(_ => _.SaveAccountManagerToPath(It.IsAny<string>(), It.IsNotNull<AccountManager>()), Times.Once);
            }
        }
    }
}
