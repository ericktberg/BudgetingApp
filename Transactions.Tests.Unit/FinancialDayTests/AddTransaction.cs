using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions;

namespace Sunsets.Transactions.Tests.Unit.FinancialDayTests
{

    [TestClass]
    public class AddTransaction
    {
        public FinancialDayTester Tester { get; } = new FinancialDayTester();

        public FinancialDay Day => Tester.Day;

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
