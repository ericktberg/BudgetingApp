using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Transactions.Tests.Unit.AccountTests
{
    [TestClass]
    public class DebtInteraction
    {
        public Calendar Calendar => Account.Calendar;
        public Account Account { get; set; }

        [TestInitialize]
        public void Init()
        {
            Account = new DebtAccount("Test");
        }

        [TestMethod]
        public void Should_Decrease_Debt_Balance_From_Income()
        {
            Account.Deposit(new Income(400), new DateTime(2000, 1, 1));

            Assert.AreEqual(-400, Account.GetBalanceFromToday());
        }

        [TestMethod]
        public void Should_Increase_Balance_When_Transferred_From()
        {
            Account secondAccount = new Account("Test2", AccountType.Liquid);

            Account.TransferFrom(new TransferFrom(400, secondAccount), new DateTime(2000, 1, 1));

            Assert.AreEqual(400, Account.GetBalanceFromToday());
            Assert.AreEqual(400, secondAccount.GetBalanceFromToday());
        }

        [TestMethod]
        public void Should_Decrease_Balance_When_Transferred_To()
        {
            Account secondAccount = new Account("Test2", AccountType.Liquid);

            Account.TransferTo(new TransferTo(400, secondAccount), new DateTime(2000, 1, 1));

            Assert.AreEqual(-400, Account.GetBalanceFromToday());
            Assert.AreEqual(-400, secondAccount.GetBalanceFromToday());
        }
    }
}
