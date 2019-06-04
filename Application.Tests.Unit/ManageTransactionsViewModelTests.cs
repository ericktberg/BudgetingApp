using System;
using System.Linq;
using BudgetingApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
}
