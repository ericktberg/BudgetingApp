using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions;
using System;

namespace Sunsets.Transactions.Tests.Unit.AccountManagerTests
{
    [TestClass]
    public class Transfer
    {
        public AccountManagerTester Tester { get; } = new AccountManagerTester();

        [TestMethod]
        public void Should_MaintainWealth()
        {
            AccountManager manager = Tester.Manager;

            Account savings = Tester.Savings;
            Account checking = Tester.Checking;

            manager.Transfer(checking, savings, 1000, new DateTime(2019, 1, 1));

            Assert.AreEqual(-1000, checking.GetBalanceFromDate(new DateTime(2019, 2, 2)));
            Assert.AreEqual(1000, savings.GetBalanceFromDate(new DateTime(2019, 2, 2)));
        }

        [TestMethod]
        public void Should_MaintainWealth_Through_DebtAccounts()
        {
            AccountManager manager = Tester.Manager;

            Account checking = Tester.Checking;
            Account debt = Tester.Credit;

            manager.Transfer(checking, debt, 1000, new DateTime(2019, 1, 1));

            Assert.AreEqual(-1000, checking.GetBalanceFromDate(new DateTime(2019, 2, 2)));
            Assert.AreEqual(-1000, debt.GetBalanceFromDate(new DateTime(2019, 2, 2)));
        }
    }
}
