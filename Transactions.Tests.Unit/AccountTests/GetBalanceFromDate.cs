using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Transactions.Tests.Unit.AccountTests
{
    [TestClass]
    public class GetBalanceFromDate
    {
        public Calendar Calendar => Account.Calendar;
        public Account Account { get; set; }

        [TestInitialize]
        public void Init()
        {
            Account = new Account("Test", AccountType.Liquid);
        }

        [TestMethod]
        public void Should_Not_Add_Transactions_After_Date()
        {
            Account.Deposit(new Income(400), new DateTime(2001, 1, 2));
            Account.Withdraw(new Expense(600), new DateTime(2001, 1, 5));

            Assert.AreEqual(400, Account.GetBalanceFromDate(new DateTime(2001, 1, 3)));
        }

        [TestMethod]
        public void Should_Not_Apply_Statements_After_Date()
        {
            Account.AddStatement(new Statement(1000), new DateTime(2001, 1, 1));
            Account.AddStatement(new Statement(2000), new DateTime(2001, 1, 5));

            Assert.AreEqual(1000, Account.GetBalanceFromDate(new DateTime(2001, 1, 3)));
        }
    }
}
