using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sunsets.Transactions.Tests.Unit.FinancialDayTests
{
    [TestClass]
    public class StartingBalance
    {
        public FinancialDayTester Tester { get; } = new FinancialDayTester();

        public FinancialDay Day => Tester.Day;

        [TestMethod]
        public void Should_StartAtZero()
        {
            FinancialDay day = Tester.GetToday();

            Assert.AreEqual(0, day.StartingBalance);
        }

        [TestMethod]
        public void Should_Statement_If_Statement_Is_EndOfDay()
        {
            FinancialDay day = Tester.GetToday();

            decimal balance = 1000;
            day.AddStatement(new Statement(balance, AddWhen.EndOfDay));

            Assert.AreEqual(balance, day.StartingBalance);
        }

        [TestMethod]
        public void Should_StartWith_StatementBalance_If_Statement_Is_StartOfDay()
        {
            FinancialDay day = Tester.GetToday();
            decimal balance = 1000;

            day.AddStatement(new Statement(balance, AddWhen.StartOfDay));
            Assert.AreEqual(balance, day.StartingBalance);
        }

        [TestMethod]
        public void Should_StartWith_PreviousDays_EndingBalance_If_NoStatementsPresent()
        {
            FinancialDay day = Tester.GetToday();

            decimal balance = 2000;
            day.PreviousDay = new MockBalance(null, balance).Object;

            Assert.AreEqual(balance, day.StartingBalance);
        }

        [TestMethod]
        public void Should_SubtractValue_From_EndOfDayStatement()
        {
            FinancialDay day = Tester.GetToday();

            decimal balance = 1000;
            day.AddStatement(new Statement(balance, AddWhen.EndOfDay));
            
            decimal income = 500;
            day.AddTransaction(new MockTransaction() { Value = income }.Object);

            Assert.AreEqual(balance - income, day.StartingBalance);
        }

        [TestMethod]
        public void Should_StartWith_PreviousDays_EndingBalance_If_ItHasStatement()
        {
            FinancialDay day = Tester.GetToday();
            day.PreviousDay = new MockBalance(0, 500) { HasStatementOnLeft = true }.Object;
            
            decimal balance = 1000;
            day.AddStatement(new Statement(balance, AddWhen.EndOfDay));

            Assert.AreEqual(500, day.StartingBalance);
        }

        [TestMethod]
        public void Should_StartWith_StatementBalance_If_PreviousDay_HasNoStatement()
        {
            FinancialDay day = Tester.GetToday();
            day.PreviousDay = new MockBalance(0, 500) { HasStatementOnLeft = false }.Object;

            decimal balance = 1000;
            day.AddStatement(new Statement(balance, AddWhen.EndOfDay));

            Assert.AreEqual(1000, day.StartingBalance);
        }

        [TestMethod]
        public void Scenario_PreviousDay_Has_OnlyStatement_StartOfDay()
        {
            var yesterday = Tester.GetYesterday();
            var today = Tester.GetToday();

            today.PreviousDay = yesterday;
            yesterday.NextDay = today;

            decimal balance = 1000;
            yesterday.AddStatement(new Statement(balance, AddWhen.StartOfDay));

            decimal income = 500;
            today.AddTransaction(new MockTransaction() { Value = income }.Object);

            Assert.AreEqual(balance, yesterday.StartingBalance);
            Assert.AreEqual(balance, yesterday.EndingBalance);
            Assert.AreEqual(balance, today.StartingBalance);
            Assert.AreEqual(balance + income, today.EndingBalance);
        }

        [TestMethod]
        public void Scenario_NoStatements()
        {
            var yesterday = Tester.GetYesterday();
            var today = Tester.GetToday();

            today.PreviousDay = yesterday;
            yesterday.NextDay = today;

            decimal income = 1000;
            yesterday.AddTransaction(new MockTransaction() { Value = income }.Object);

            decimal expense = -500;
            today.AddTransaction(new MockTransaction() { Value = expense }.Object);

            Assert.AreEqual(0, yesterday.StartingBalance);
            Assert.AreEqual(income, yesterday.EndingBalance);
            Assert.AreEqual(income, today.StartingBalance);
            Assert.AreEqual(income + expense, today.EndingBalance);
        }

        [TestMethod]
        public void Scenario_NextDay_Has_OnlyStatement_EndOfDay()
        {
            var today = Tester.GetToday();
            var tomorrow = Tester.GetTomorrow();

            today.NextDay = tomorrow;
            tomorrow.PreviousDay = today;

            decimal balance = 1000;
            tomorrow.AddStatement(new Statement(balance, AddWhen.EndOfDay));

            decimal income = 500;
            today.AddTransaction(new MockTransaction() { Value = income }.Object);
            
            Assert.AreEqual(balance - income, today.StartingBalance);
            Assert.AreEqual(balance, today.EndingBalance);
            Assert.AreEqual(balance, tomorrow.StartingBalance);
            Assert.AreEqual(balance, tomorrow.EndingBalance);
        }

        [TestMethod]
        public void Scenario_TwoStatements_Start_End()
        {
            var today = Tester.GetToday();
            var tomorrow = Tester.GetTomorrow();

            today.NextDay = tomorrow;
            tomorrow.PreviousDay = today;

            decimal todayBalance = 1000;
            today.AddStatement(new Statement(todayBalance, AddWhen.StartOfDay));

            decimal tomorrowBalance = 2000;
            tomorrow.AddStatement(new Statement(tomorrowBalance, AddWhen.EndOfDay));

            Assert.AreEqual(todayBalance, today.StartingBalance);
            Assert.AreEqual(todayBalance, today.EndingBalance);
            Assert.AreEqual(todayBalance, tomorrow.StartingBalance);
            Assert.AreEqual(tomorrowBalance, tomorrow.EndingBalance);
        }

        [TestMethod]
        public void Scenario_TwoStatements_Start_Start()
        {
            var today = Tester.GetToday();
            var tomorrow = Tester.GetTomorrow();

            today.NextDay = tomorrow;
            tomorrow.PreviousDay = today;

            decimal todayBalance = 1000;
            today.AddStatement(new Statement(todayBalance, AddWhen.StartOfDay));

            decimal tomorrowBalance = 2000;
            tomorrow.AddStatement(new Statement(tomorrowBalance, AddWhen.StartOfDay));

            Assert.AreEqual(todayBalance, today.StartingBalance);
            Assert.AreEqual(todayBalance, today.EndingBalance);
            Assert.AreEqual(tomorrowBalance, tomorrow.StartingBalance);
            Assert.AreEqual(tomorrowBalance, tomorrow.EndingBalance);
        }

        [TestMethod]
        public void Scenario_TwoStatements_End_Start()
        {
            var today = Tester.GetToday();
            var tomorrow = Tester.GetTomorrow();

            today.NextDay = tomorrow;
            tomorrow.PreviousDay = today;

            decimal todayBalance = 1000;
            today.AddStatement(new Statement(todayBalance, AddWhen.EndOfDay));

            decimal tomorrowBalance = 2000;
            tomorrow.AddStatement(new Statement(tomorrowBalance, AddWhen.StartOfDay));

            Assert.AreEqual(todayBalance, today.StartingBalance);
            Assert.AreEqual(todayBalance, today.EndingBalance);
            Assert.AreEqual(tomorrowBalance, tomorrow.StartingBalance);
            Assert.AreEqual(tomorrowBalance, tomorrow.EndingBalance);
        }

        [TestMethod]
        public void Scenario_TwoStatements_End_End()
        {
            var today = Tester.GetToday();
            var tomorrow = Tester.GetTomorrow();

            today.NextDay = tomorrow;
            tomorrow.PreviousDay = today;

            decimal todayBalance = 1000;
            today.AddStatement(new Statement(todayBalance, AddWhen.EndOfDay));

            decimal tomorrowBalance = 2000;
            tomorrow.AddStatement(new Statement(tomorrowBalance, AddWhen.EndOfDay));

            Assert.AreEqual(todayBalance, today.StartingBalance);
            Assert.AreEqual(todayBalance, today.EndingBalance);
            Assert.AreEqual(todayBalance, tomorrow.StartingBalance);
            Assert.AreEqual(tomorrowBalance, tomorrow.EndingBalance);
        }
    }
}
