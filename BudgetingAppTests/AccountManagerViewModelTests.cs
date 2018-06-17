using System;
using System.Linq;
using BudgetingApp.Model.FileManager;
using BudgetingApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Transactions;
using Transactions.Accounts;

namespace BudgetingAppTests
{
    public class MockFileManager : IManageFiles
    {
        public string DefaultFileName => "Default";

        public string DefaultFileLocation => "Default";

        public string DefaultFilePath => "Default";

        public AccountManager GetAccountManagerFromPath(string filePath)
        {
            return new AccountManager();
        }

        public bool IsSaved { get; set; }

        public bool SaveAccountManagerToPath(string filePath, AccountManager account)
        {
            IsSaved = true;
            return true;
        }
    }

    [TestClass]
    public partial class AccountManagerViewModelTests
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

        [TestClass]
        public class TransactionTypeTests : AccountManagerViewModelTests
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
        public class AddAccount : AccountManagerViewModelTests
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

        [TestClass]
        public class AddStatement : AccountManagerViewModelTests
        {
            [TestMethod]
            public void Should_Keep_Days_Synchronized_When_Adding_Statement()
            {
                Assert.AreEqual(0, ViewModel.AccountManager.Calendar.Days.Count());
                Assert.AreEqual(0, ViewModel.Days.Count);
                var account = new Account("TestAccount");

                ViewModel.AddAccount(account);

                ViewModel.AddStatement(new Statement(1000, account), new DateTime(2000, 1, 1));

                Assert.AreEqual(1, ViewModel.AccountManager.Calendar.Days.Count());
                Assert.AreEqual(1, ViewModel.Days.Count);
            }
        }
    }
}
