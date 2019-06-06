using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sunsets.Transactions.Tests.Unit.FinancialDayTests
{
    [TestClass]
    public class EndingBalance
    {
        public FinancialDayTester Tester { get; } = new FinancialDayTester();
        
        [TestMethod]
        public void Should_EndWithZero()
        {
            FinancialDay day = Tester.GetToday();

            Assert.AreEqual(0, day.EndingBalance);
        }

        [TestMethod]
        public void Should_EndWithStatementBalance_When_EndOfDay()
        {
            FinancialDay day = Tester.GetToday();
            decimal balance = 1000;

            day.AddStatement(new Statement(balance, AddWhen.EndOfDay));
            Assert.AreEqual(balance, day.EndingBalance);
        }

        [TestMethod]
        public void Should_EndWithStatementBalance_When_StartOfDay()
        {
            FinancialDay day = Tester.GetToday();
            decimal balance = 1000;

            day.AddStatement(new Statement(balance, AddWhen.StartOfDay));
            Assert.AreEqual(balance, day.EndingBalance);
        }

        [TestMethod]
        public void Should_AddTransactionValue()
        {
            FinancialDay day = Tester.GetToday();
            decimal income = 500;

            day.AddTransaction(new MockTransaction() { Value = income });
            Assert.AreEqual(income, day.EndingBalance);
        }

        [TestMethod]
        public void Should_SubtractNegativeTransactionValue()
        {
            FinancialDay day = Tester.GetToday();
            decimal expense = -500;

            day.AddTransaction(new MockTransaction() { Value = expense });
            Assert.AreEqual(expense, day.EndingBalance);
        }

        [TestMethod]
        public void Should_EndWithStatementBalance_Over_Transactions_When_EndOfDay()
        {
            FinancialDay day = Tester.GetToday();
            decimal balance = 1000;

            day.AddStatement(new Statement(balance, AddWhen.EndOfDay));

            decimal income = 500;
            day.AddTransaction(new MockTransaction() { Value = income });

            Assert.AreEqual(balance, day.EndingBalance);
        }

        [TestMethod]
        public void Should_AddBalance_To_StartOfDayStatement()
        {
            FinancialDay day = Tester.GetToday();
            decimal balance = 1000;

            day.AddStatement(new Statement(balance, AddWhen.StartOfDay));

            decimal income = 500;

            day.AddTransaction(new MockTransaction() { Value = income });

            Assert.AreEqual(balance + income, day.EndingBalance);
        }

        [TestMethod]
        public void Should_AddTransactions_To_PreviousDays_EndingBalance()
        {
            FinancialDay day = Tester.GetToday();
            decimal balance = 2000;

            day.PreviousDay = new MockBalance(null, balance)
            {
                HasStatementOnLeft = true
            };
            Assert.AreEqual(balance, day.StartingBalance);

            decimal income = 500;
            day.AddTransaction(new MockTransaction() { Value = income });

            Assert.AreEqual(balance + income, day.EndingBalance);
        }
    }
}
