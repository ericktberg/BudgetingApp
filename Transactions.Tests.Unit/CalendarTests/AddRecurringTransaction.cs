using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Sunsets.Transactions.Tests.Unit.CalendarTests
{
    [TestClass]
    public class AddRecurringTransaction
    {
        CalendarTester Tester { get; } = new CalendarTester();

        [TestMethod]
        public void Should_NotUpdate_EmptyDays()
        {
            Calendar c = Tester.NewCalendar();

            c.AddRecurringTransaction(Tester.NewRecurringTransaction());

            Assert.AreEqual(0, c.Days.Count());
        }

        [TestMethod]
        public void Should_Add_ToRecurringList()
        {
            Calendar c = Tester.NewCalendar();

            c.AddRecurringTransaction(Tester.NewRecurringTransaction());

            Assert.AreEqual(1, c.RecurringTransactions.Count());
        }

        [TestMethod]
        public void Should_AddTransaction_To_OnlyDay()
        {
            Calendar c = Tester.NewCalendar();
            DateTime date = Tester.StartDate;

            var day = c.GetDayForDate(date);
            Assert.AreEqual(0, day.TransactionCollection.Count);

            Tester.MockFrequency.DateCollection.Add(date);

            c.AddRecurringTransaction(Tester.NewRecurringTransaction());
            Assert.AreEqual(1, day.TransactionCollection.Count);
        }
    }
}
