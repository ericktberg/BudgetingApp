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

            c.AddRecurringTransaction(Tester.MockRecurring.Object);

            Assert.AreEqual(0, c.Days.Count());
        }

        [TestMethod]
        public void Should_Add_ToRecurringList()
        {
            Calendar c = Tester.NewCalendar();

            c.AddRecurringTransaction(Tester.MockRecurring.Object);

            Assert.AreEqual(1, c.RecurringTransactions.Count());
        }

        [TestMethod]
        public void Should_AddTransaction_To_OnlyDay()
        {
            Calendar c = Tester.NewCalendar();
            DateTime date = Tester.StartDate;

            var day = c.GetDayForDate(date);
            Assert.AreEqual(0, day.TransactionCollection.Count);

            Tester.MockRecurring.Dates.Add(date);

            c.AddRecurringTransaction(Tester.MockRecurring.Object);
            Assert.AreEqual(1, day.TransactionCollection.Count);
        }

        [TestMethod]
        public void Should_CreateNewDays_FromFirstElement_Until_LastDay()
        {
            Calendar c = Tester.NewCalendar();
            
            DateTime date1 = Tester.StartDate;
            DateTime date2 = new DateTime(2019, 2, 3);
            DateTime date3 = new DateTime(2019, 5, 3);
            DateTime date4 = new DateTime(2019, 6, 1);

            c.GetDayForDate(date4);
            
            Tester.MockRecurring.Dates.Add(date1);
            Tester.MockRecurring.Dates.Add(date2);
            Tester.MockRecurring.Dates.Add(date3);

            c.AddRecurringTransaction(Tester.MockRecurring.Object);

            Assert.AreEqual(4, c.Days.Count());

            Assert.AreEqual(date1, c.Days.ElementAt(0).Date);
            Assert.AreEqual(date2, c.Days.ElementAt(1).Date);
            Assert.AreEqual(date3, c.Days.ElementAt(2).Date);
            Assert.AreEqual(date4, c.Days.ElementAt(3).Date);
        }

        [TestMethod]
        public void Should_CreateNoDays_If_LastCalendarDay_Is_BeforeFirstDate()
        {
            Calendar c = Tester.NewCalendar();

            DateTime date1 = Tester.StartDate;
            DateTime date2 = new DateTime(2019, 2, 3);
            DateTime date3 = new DateTime(2019, 5, 3);
            DateTime date4 = new DateTime(2018, 6, 1);

            c.GetDayForDate(date4);

            Tester.MockRecurring.Dates.Add(date1);
            Tester.MockRecurring.Dates.Add(date2);
            Tester.MockRecurring.Dates.Add(date3);

            c.AddRecurringTransaction(Tester.MockRecurring.Object);

            Assert.AreEqual(1, c.Days.Count());
            
            Assert.AreEqual(date4, c.Days.ElementAt(0).Date);
        }

        [TestMethod]
        public void Should_CreateDays_ForEvery_AlreadyEnumeratedElement()
        {
            Calendar c = Tester.NewCalendar();
            
            DateTime date1 = Tester.StartDate;
            DateTime date2 = new DateTime(2019, 2, 3);
            DateTime date3 = new DateTime(2019, 5, 3);
            
            Tester.MockRecurring.Dates.Add(date1);
            Tester.MockRecurring.Dates.Add(date2);
            Tester.MockRecurring.Dates.Add(date3);

            Tester.MockRecurring.Object.EnumerateElementsUntilDate(date3.AddDays(1));

            c.AddRecurringTransaction(Tester.MockRecurring.Object);

            Assert.AreEqual(3, c.Days.Count());

            Assert.AreEqual(date1, c.Days.ElementAt(0).Date);
            Assert.AreEqual(date2, c.Days.ElementAt(1).Date);
            Assert.AreEqual(date3, c.Days.ElementAt(2).Date);
        }
    }
}
