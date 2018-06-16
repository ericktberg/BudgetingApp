using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Transactions;
using Transactions.Accounts;

namespace TransactionsTests
{
    [TestClass]
    public class FinancialDayTest
    {
        public FinancialDay Day { get; } = new FinancialDay(DateTime.Now);

        [TestMethod]
        public void Should_Initialize_Empty()
        {
            Assert.AreEqual(0, Day.TransactionCollection.Count);
            Assert.AreEqual(0, Day.Statements.Count);
        }


        [TestClass]
        public class GetTransactionsForAccount : FinancialDayTest
        {
            [TestMethod]
            public void Should_Retrieve_Added_Transaction()
            {
                Account a = new Account("Test");

                Day.AddTransaction(new Income(400, a));

                Assert.AreEqual(1, Day.TransactionCollection.Count);
                Assert.AreEqual(1, Day.GetTransactionsForAccount(a).Count());
                Assert.AreEqual(400, Day.GetTransactionsForAccount(a).First().Amount);
            }
        }

        [TestClass]
        public class GetStatementForAccount : FinancialDayTest
        {
            [TestMethod]
            public void Should_Return_Null_When_Statement_Missing()
            {
                Assert.IsNull(Day.GetStatementForAccount(new Account("Test")));
            }

            [TestMethod]
            public void Should_Return_Statement()
            {
                Account a = new Account("Test");

                Day.AddStatement(new Statement(1000, a));

                Assert.AreEqual(1, Day.Statements.Count);
                Assert.IsNotNull(Day.GetStatementForAccount(a));
                Assert.AreEqual(1000, Day.GetStatementForAccount(a).Balance);
            }
        }
    }
}
