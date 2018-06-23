using System;
using System.Linq;
using BudgetingApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Transactions;
using Transactions.Accounts;

namespace BudgetingAppTests
{
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
}
