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
    }
}
