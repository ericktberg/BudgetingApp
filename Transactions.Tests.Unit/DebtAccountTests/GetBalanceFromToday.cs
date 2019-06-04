using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunsets.Transactions;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Transactions.Tests.Unit.DebtAccountTests
{
    [TestClass]
    public class GetBalanceFromToday
    {
        public static Calendar Calendar { get; } = new Calendar();

        public DebtAccount Account { get; set; } = new DebtAccount("Test");

        [TestMethod]
        public void Should_Decrease_Balance_From_Deposits()
        {
            Assert.AreEqual(-400, Account.GetDelta(400));
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
