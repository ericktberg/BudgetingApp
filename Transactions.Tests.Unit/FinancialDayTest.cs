using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Sunsets.Transactions.Tests.Unit
{
    [TestClass]
    public class FinancialDayTest
    {
        public FinancialDay Day { get; } = new FinancialDay(DateTime.Now);

        [TestClass]
        public class Constructor : FinancialDayTest
        {
            [TestMethod]
            public void Should_Initialize_Empty()
            {
                Assert.AreEqual(0, Day.TransactionCollection.Count);
                Assert.AreEqual(0, Day.Statements.Count);
            }
        }

        [TestClass]
        public class AddTransaction : FinancialDayTest
        {
            [TestMethod]
            public void Should_NotAdd_When_TransactionExists()
            {
                Transaction income = new Income(300);

                Day.AddTransaction(income);
                Day.AddTransaction(income);

                Assert.AreEqual(1, Day.TransactionCollection.Count);
            }

            [TestMethod]
            public void Should_NotAdd_When_TransactionWithGuidExists()
            {
                Transaction one = new Income(300);

                Transaction two = new Expense(400, one.TransactionGuid);

                Day.AddTransaction(one);
                Day.AddTransaction(two);

                Assert.AreEqual(1, Day.TransactionCollection.Count);
            }

            [TestMethod]
            public void Should_Add_When_AllUniqueTransactions()
            {
                Transaction one = new Income(300);

                Transaction two = new Expense(400);
                Transaction three = new Income(500);

                Day.AddTransaction(one);
                Day.AddTransaction(two);
                Day.AddTransaction(three);

                Assert.AreEqual(3, Day.TransactionCollection.Count);
            }
        }
    }
}
