using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Sunsets.Transactions.Tests.Unit.RecurringTransactionTests
{
    [TestClass]
    public class Constructor
    {
        RecurringTransactionTester Tester { get; } = new RecurringTransactionTester();

        [TestMethod]
        public void Should_StartWith_EmptyElements()
        {
            RecurringTransaction t = new RecurringTransaction(new Income(500), Tester.MockFrequency.Object, new DateTime(2019, 1, 1), null);

            Assert.AreEqual(0, t.Elements.Count());
        }
    }
}