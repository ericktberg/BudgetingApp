using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions;

namespace Sunsets.Transactions.Tests.Unit.FinancialDayTests
{
    [TestClass]
    public class Constructor
    {
        public FinancialDayTester Tester { get; } = new FinancialDayTester();

        public FinancialDay Day => Tester.Day;

        [TestMethod]
        public void Should_Initialize_Empty()
        {
            Assert.AreEqual(0, Day.TransactionCollection.Count);
            Assert.IsNull(Day.Statement);
        }
    }
}
