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
    public class GetBalanceFromDate
    {
        public static Calendar Calendar { get; } = new Calendar();

        public DebtAccount Account { get; set; } = new DebtAccount("Test");

        public DateTime Date { get; } = new DateTime(2005, 5, 5);

        [TestMethod]
        public void Should_Decrease_Balance_From_Deposits()
        {
            Assert.AreEqual(-400, Account.GetDelta(400));
        }

        [TestMethod]
        public void Should_Decrease_Debt_Balance_From_Income()
        {
            Account.AddTransaction(new Income(400), new DateTime(2000, 1, 1));

            Assert.AreEqual(-400, Account.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void Should_Increase_Balance_When_Transferred_From()
        {
            Account secondAccount = new Account("Test2", AccountType.Liquid);

            Account.AddTransaction(new TransferFrom(400, secondAccount), new DateTime(2000, 1, 1));

            Assert.AreEqual(400, Account.GetBalanceFromDate(Date));
            Assert.AreEqual(400, secondAccount.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void Should_Decrease_Balance_When_Transferred_To()
        {
            Account secondAccount = new Account("Test2", AccountType.Liquid);

            Account.AddTransaction(new TransferTo(400, secondAccount), new DateTime(2000, 1, 1));

            Assert.AreEqual(-400, Account.GetBalanceFromDate(Date));
            Assert.AreEqual(-400, secondAccount.GetBalanceFromDate(Date));
        }
    }
}
