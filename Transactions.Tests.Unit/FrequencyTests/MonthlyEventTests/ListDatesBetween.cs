using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Sunsets.Transactions.Tests.Unit.FrequencyTests.MonthlyEventTests
{
    [TestClass]
    public class ListDatesBetween
    {
        [TestMethod]
        public void Should_BeEmpty()
        {
            MonthlyEvent e = new MonthlyEvent(2);

            Assert.AreEqual(0, e.ListDatesBetween(new DateTime(2019, 1, 3), new DateTime(2019, 1, 7)).Count());
        }

        [TestMethod]
        public void Should_ListSingleDate()
        {
            MonthlyEvent e = new MonthlyEvent(2);

            var list = e.ListDatesBetween(new DateTime(2019, 1, 1), new DateTime(2019, 1, 7));

            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(new DateTime(2019, 1, 2), list.First());
        }

        [TestMethod]
        public void Should_ListConsecutiveDates()
        {
            MonthlyEvent e = new MonthlyEvent(2);

            var list = e.ListDatesBetween(new DateTime(2019, 1, 1), new DateTime(2019, 3, 7));

            Assert.AreEqual(3, list.Count());
            Assert.AreEqual(new DateTime(2019, 1, 2), list.ElementAt(0));
            Assert.AreEqual(new DateTime(2019, 2, 2), list.ElementAt(1));
            Assert.AreEqual(new DateTime(2019, 3, 2), list.ElementAt(2));
        }

        [TestMethod]
        public void Should_WrapAroundYears()
        {
            MonthlyEvent e = new MonthlyEvent(2);

            var list = e.ListDatesBetween(new DateTime(2018, 12, 27), new DateTime(2019, 1, 7));

            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(new DateTime(2019, 1, 2), list.First());
        }

        [TestMethod]
        public void Should_WrapYears_From_StartingMonth()
        {
            MonthlyEvent e = new MonthlyEvent(2);

            var list = e.ListDatesBetween(new DateTime(2018, 12, 1), new DateTime(2019, 1, 1));

            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(new DateTime(2018, 12, 2), list.First());
        }

        [TestMethod]
        public void Should_AddLastDayOfMonth_If_Day_Is_OutOfBounds()
        {
            MonthlyEvent e = new MonthlyEvent(31);

            var list = e.ListDatesBetween(new DateTime(2019, 2, 15), new DateTime(2019, 3, 7));

            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(new DateTime(2019, 2, 28), list.First());
        }
    }
}
