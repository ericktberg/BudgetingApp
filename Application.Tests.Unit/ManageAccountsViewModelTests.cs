using System;
using System.Linq;
using BudgetingApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Transactions;
using Transactions.Accounts;

namespace BudgetingAppTests
{
    [TestClass]
    public class ManageAccountsViewModelTests
    {
        public AccountManager AccountManager { get; set; } = new AccountManager();

        public ManageAccountsViewModel ViewModel { get; set; }

        [TestClass]
        public class AddAccount : ManageAccountsViewModelTests
        {
            [TestMethod]
            public void Should_Keep_Lists_Synchronized()
            {
                ViewModel = new ManageAccountsViewModel(AccountManager);

                int beforeCount = ViewModel.AccountManager.Accounts.Count;

                Assert.AreEqual(beforeCount, ViewModel.Accounts.Count);

                ViewModel.AddAccount(new Account("TestAccount"));

                int afterCount = ViewModel.AccountManager.Accounts.Count;

                Assert.AreEqual(beforeCount + 1, afterCount);
                Assert.AreEqual(afterCount, ViewModel.Accounts.Count);
            }
        }

        [TestClass]
        public class RemoveAccount : ManageAccountsViewModelTests
        {
            [TestMethod]
            public void Should_KeepListsSynchronized_After_RemovingAnAccount()
            {
                ViewModel = new ManageAccountsViewModel(AccountManager);

                var account = new Account("TestAccount");

                ViewModel.AddAccount(account);

                ViewModel.RemoveAccount(ViewModel.Accounts[0]);

                Assert.AreEqual(0, ViewModel.AccountManager.Accounts.Count);
                Assert.AreEqual(0, ViewModel.Accounts.Count);
            }
        }

        [TestClass]
        public class NetBalance : ManageAccountsViewModelTests
        {
            [TestMethod]
            public void Should_BeZero_When_NoAccounts()
            {
                ViewModel = new ManageAccountsViewModel(AccountManager);

                Assert.AreEqual(0, ViewModel.NetBalance);
            }

            [TestMethod]
            public void Should_BeZero_When_NoAccountsSelected()
            {
                Account account = new Account("NotSelected");
                AccountManager.Accounts.Add(account);
                AccountManager.AddStatement(new Statement(1000, account), new DateTime(2000, 1, 1));
                ViewModel = new ManageAccountsViewModel(AccountManager);

                Assert.AreEqual(0, ViewModel.NetBalance);
            }

            [TestMethod]
            public void Should_SumSelectedAccountBalances()
            {
                Account account = new Account("SelectedLiquid", AccountType.Liquid);
                Account account2 = new Account("SelectedInvested", AccountType.Invested);
                AccountManager.Accounts.Add(account);
                AccountManager.Accounts.Add(account2);
                AccountManager.AddStatement(new Statement(1000, account), new DateTime(2000, 1, 1));
                AccountManager.AddStatement(new Statement(2300, account2), new DateTime(2001, 1, 1));
                ViewModel = new ManageAccountsViewModel(AccountManager);

                ViewModel.SelectNetWorth();

                Assert.AreEqual(3300, ViewModel.NetBalance);
            }

            [TestMethod]
            public void Should_SubtractBalances_When_TheyAreDebtAccounts()
            {
                Account liquid = new Account("SelectedLiquid", AccountType.Liquid);
                Account invested = new Account("SelectedInvested", AccountType.Invested);
                Account debt = new Account("Debt", AccountType.Debt);
                AccountManager.Accounts.Add(liquid);
                AccountManager.Accounts.Add(invested);
                AccountManager.Accounts.Add(debt);
                AccountManager.AddStatement(new Statement(1000, liquid), new DateTime(2000, 1, 1));
                AccountManager.AddStatement(new Statement(2300, invested), new DateTime(2001, 1, 1));
                AccountManager.AddStatement(new Statement(1200, debt), new DateTime(2002, 1, 1));
                ViewModel = new ManageAccountsViewModel(AccountManager);

                ViewModel.SelectNetWorth();

                Assert.AreEqual(2100, ViewModel.NetBalance);
            }
        } 


        [TestClass]
        public class NetWorth : ManageAccountsViewModelTests
        {
            [TestMethod]
            public void Should_SelectedAllAccounts()
            {
                Account liquid = new Account("Liquid", AccountType.Liquid);
                Account invested = new Account("Invested", AccountType.Invested);
                Account debt = new Account("Debt", AccountType.Debt);
                Account property = new Account("Property", AccountType.Property);

                AccountManager.Accounts.Add(liquid);
                AccountManager.Accounts.Add(invested);
                AccountManager.Accounts.Add(debt);
                AccountManager.Accounts.Add(property);

                ViewModel = new ManageAccountsViewModel(AccountManager);
                Assert.AreEqual(0, ViewModel.Accounts.Count(a => a.IsSelected));

                ViewModel.SelectNetWorth();

                Assert.AreEqual(4, ViewModel.Accounts.Count(a => a.IsSelected));
            }
        }

        [TestClass]
        public class Assets : ManageAccountsViewModelTests
        {
            [TestMethod]
            public void Should_SelectAllAssetAccounts()
            {
                Account liquid = new Account("Liquid", AccountType.Liquid);
                Account invested = new Account("Invested", AccountType.Invested);
                Account debt = new Account("Debt", AccountType.Debt);
                Account property = new Account("Property", AccountType.Property);

                AccountManager.Accounts.Add(liquid);
                AccountManager.Accounts.Add(invested);
                AccountManager.Accounts.Add(debt);
                AccountManager.Accounts.Add(property);

                ViewModel = new ManageAccountsViewModel(AccountManager);
                Assert.AreEqual(0, ViewModel.Accounts.Count(a => a.IsSelected));

                ViewModel.SelectAssets();

                Assert.AreEqual(3, ViewModel.Accounts.Count(a => a.IsSelected));
            }
        }

        [TestClass]
        public class Liabilities : ManageAccountsViewModelTests
        {
            [TestMethod]
            public void Should_SelectAllLiabilityAccounts()
            {
                Account liquid = new Account("Liquid", AccountType.Liquid);
                Account invested = new Account("Invested", AccountType.Invested);
                Account debt = new Account("Debt", AccountType.Debt);
                Account property = new Account("Property", AccountType.Property);

                AccountManager.Accounts.Add(liquid);
                AccountManager.Accounts.Add(invested);
                AccountManager.Accounts.Add(debt);
                AccountManager.Accounts.Add(property);

                ViewModel = new ManageAccountsViewModel(AccountManager);
                Assert.AreEqual(0, ViewModel.Accounts.Count(a => a.IsSelected));

                ViewModel.SelectLiabilities();

                Assert.AreEqual(1, ViewModel.Accounts.Count(a => a.IsSelected));
                Assert.IsTrue(ViewModel.Accounts[2].IsSelected);
            }
        }

        [TestClass]
        public class Liquid : ManageAccountsViewModelTests
        {
            [TestMethod]
            public void Should_SelectAllLiquidAccounts()
            {
                Account liquid = new Account("Liquid", AccountType.Liquid);
                Account invested = new Account("Invested", AccountType.Invested);
                Account debt = new Account("Debt", AccountType.Debt);
                Account property = new Account("Property", AccountType.Property);

                AccountManager.Accounts.Add(liquid);
                AccountManager.Accounts.Add(invested);
                AccountManager.Accounts.Add(debt);
                AccountManager.Accounts.Add(property);

                ViewModel = new ManageAccountsViewModel(AccountManager);
                Assert.AreEqual(0, ViewModel.Accounts.Count(a => a.IsSelected));

                ViewModel.SelectLiquid();

                Assert.AreEqual(1, ViewModel.Accounts.Count(a => a.IsSelected));
                Assert.IsTrue(ViewModel.Accounts[0].IsSelected);
            }
        }

        [TestClass]
        public class Invested : ManageAccountsViewModelTests
        {
            [TestMethod]
            public void Should_SelectAllInvestedAccounts()
            {
                Account liquid = new Account("Liquid", AccountType.Liquid);
                Account invested = new Account("Invested", AccountType.Invested);
                Account debt = new Account("Debt", AccountType.Debt);
                Account property = new Account("Property", AccountType.Property);

                AccountManager.Accounts.Add(liquid);
                AccountManager.Accounts.Add(invested);
                AccountManager.Accounts.Add(debt);
                AccountManager.Accounts.Add(property);

                ViewModel = new ManageAccountsViewModel(AccountManager);
                Assert.AreEqual(0, ViewModel.Accounts.Count(a => a.IsSelected));

                ViewModel.SelectInvested();

                Assert.AreEqual(1, ViewModel.Accounts.Count(a => a.IsSelected));
                Assert.IsTrue(ViewModel.Accounts[1].IsSelected);
            }
        }

        [TestClass]
        public class Property : ManageAccountsViewModelTests
        {
            [TestMethod]
            public void Should_SelectAllPropertyAccounts()
            {
                Account liquid = new Account("Liquid", AccountType.Liquid);
                Account invested = new Account("Invested", AccountType.Invested);
                Account debt = new Account("Debt", AccountType.Debt);
                Account property = new Account("Property", AccountType.Property);

                AccountManager.Accounts.Add(liquid);
                AccountManager.Accounts.Add(invested);
                AccountManager.Accounts.Add(debt);
                AccountManager.Accounts.Add(property);

                ViewModel = new ManageAccountsViewModel(AccountManager);
                Assert.AreEqual(0, ViewModel.Accounts.Count(a => a.IsSelected));

                ViewModel.SelectProperty();

                Assert.AreEqual(1, ViewModel.Accounts.Count(a => a.IsSelected));
                Assert.IsTrue(ViewModel.Accounts[3].IsSelected);
            }
        }

        [TestClass]
        public class Debt : ManageAccountsViewModelTests
        {
            [TestMethod]
            public void Should_SelectAllDebtAccounts()
            {
                Account liquid = new Account("Liquid", AccountType.Liquid);
                Account invested = new Account("Invested", AccountType.Invested);
                Account debt = new Account("Debt", AccountType.Debt);
                Account property = new Account("Property", AccountType.Property);

                AccountManager.Accounts.Add(liquid);
                AccountManager.Accounts.Add(invested);
                AccountManager.Accounts.Add(debt);
                AccountManager.Accounts.Add(property);

                ViewModel = new ManageAccountsViewModel(AccountManager);
                Assert.AreEqual(0, ViewModel.Accounts.Count(a => a.IsSelected));

                ViewModel.SelectDebt();

                Assert.AreEqual(1, ViewModel.Accounts.Count(a => a.IsSelected));
                Assert.IsTrue(ViewModel.Accounts[2].IsSelected);
            }
        }
    }
}
