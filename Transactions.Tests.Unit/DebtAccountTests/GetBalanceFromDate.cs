using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunsets.Transactions;

namespace Sunsets.Transactions.Tests.Unit.DebtAccountTests
{
    [TestClass]
    public class GetBalanceFromDate
    {
        public static Calendar Calendar { get; } = new Calendar();

        public LiabilityAccount Account { get; set; } = new LiabilityAccount("Test");

        public DateTime Date { get; } = new DateTime(2005, 5, 5);

        [TestMethod]
        public void Should_DecreaseBalance_From_PositiveValue()
        {
            Account.AddTransaction(new MockTransaction() { Value = 400 }.Object, new DateTime(2000, 1, 1));

            Assert.AreEqual(-400, Account.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void Should_IncreaseBalance_From_NegativeValue()
        {
            Account.AddTransaction(new MockTransaction() { Value = -400 }.Object, new DateTime(2000, 1, 1));

            Assert.AreEqual(400, Account.GetBalanceFromDate(Date));
        }
    }
}
