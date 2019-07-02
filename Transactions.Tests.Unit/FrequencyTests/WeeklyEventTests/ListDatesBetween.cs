using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Sunsets.Transactions.Tests.Unit.FrequencyTests.WeeklyEventTests
{
    [TestClass]
    public class ListDatesBetween
    {
        [TestMethod]
        public void Should_BeEmpty()
        {
            WeeklyEvent e = new WeeklyEvent(DayOfWeek.Tuesday);

            var list = e.ListDatesBetween(new DateTime(2019, 6, 5), new DateTime(2019, 6, 8));

            Assert.AreEqual(0, list.Count());
        }

        [TestMethod]
        public void Should_ReturnWeekday_BetweenDates()
        {
            WeeklyEvent e = new WeeklyEvent(DayOfWeek.Tuesday);

            var list = e.ListDatesBetween(new DateTime(2019, 6, 1), new DateTime(2019, 6, 8));

            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(new DateTime(2019, 6, 4), list.First());
        }

        [TestMethod]
        public void Should_ReturnCoincidentDates()
        {
            WeeklyEvent e = new WeeklyEvent(DayOfWeek.Tuesday);

            var list = e.ListDatesBetween(new DateTime(2019, 6, 4), new DateTime(2019, 6, 4));

            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(new DateTime(2019, 6, 4), list.First());
        }

        [TestMethod]
        public void Should_ReturnConsecutiveDates()
        {
            WeeklyEvent e = new WeeklyEvent(DayOfWeek.Tuesday);

            var list = e.ListDatesBetween(new DateTime(2019, 6, 1), new DateTime(2019, 6, 15));

            Assert.AreEqual(2, list.Count());
            Assert.AreEqual(new DateTime(2019, 6, 4), list.ElementAt(0));
            Assert.AreEqual(new DateTime(2019, 6, 11), list.ElementAt(1));
        }
    }
}
