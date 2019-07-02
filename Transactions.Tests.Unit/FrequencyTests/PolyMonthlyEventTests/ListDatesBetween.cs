using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Sunsets.Transactions.Tests.Unit.FrequencyTests.PolyMonthlyEventTests
{
    [TestClass]
    public class ListDatesBetween
    {
        [TestMethod]
        public void Should_BeEmpty()
        {
            PolyMonthlyEvent e = new PolyMonthlyEvent(8, 16, 24);

            var list = e.ListDatesBetween(new DateTime(2019, 1, 9), new DateTime(2019, 1, 15));

            Assert.AreEqual(0, list.Count());
        }

        [TestMethod]
        public void Should_ListFirstDate()
        {
            PolyMonthlyEvent e = new PolyMonthlyEvent(8, 16, 24);

            var list = e.ListDatesBetween(new DateTime(2019, 1, 1), new DateTime(2019, 1, 15));

            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(new DateTime(2019, 1, 8), list.ElementAt(0));
        }

        [TestMethod]
        public void Should_ListFirstTwoDates_InMonth()
        {
            PolyMonthlyEvent e = new PolyMonthlyEvent(8, 16, 24);

            var list = e.ListDatesBetween(new DateTime(2019, 1, 1), new DateTime(2019, 1, 18));

            Assert.AreEqual(2, list.Count());
            Assert.AreEqual(new DateTime(2019, 1, 8), list.ElementAt(0));
            Assert.AreEqual(new DateTime(2019, 1, 16), list.ElementAt(1));
        }

        [TestMethod]
        public void Should_ListAllThreeDates_InMonth()
        {
            PolyMonthlyEvent e = new PolyMonthlyEvent(8, 16, 24);

            var list = e.ListDatesBetween(new DateTime(2019, 1, 1), new DateTime(2019, 1, 28));

            Assert.AreEqual(3, list.Count());
            Assert.AreEqual(new DateTime(2019, 1, 8), list.ElementAt(0));
            Assert.AreEqual(new DateTime(2019, 1, 16), list.ElementAt(1));
            Assert.AreEqual(new DateTime(2019, 1, 24), list.ElementAt(2));
        }

        [TestMethod]
        public void Should_OrderDates_From_MultipleMonths()
        {
            PolyMonthlyEvent e = new PolyMonthlyEvent(8, 16, 24);

            var list = e.ListDatesBetween(new DateTime(2019, 1, 9), new DateTime(2019, 3, 17));

            Assert.AreEqual(7, list.Count());
            Assert.AreEqual(new DateTime(2019, 1, 16), list.ElementAt(0));
            Assert.AreEqual(new DateTime(2019, 1, 24), list.ElementAt(1));
            Assert.AreEqual(new DateTime(2019, 2, 8), list.ElementAt(2));
            Assert.AreEqual(new DateTime(2019, 2, 16), list.ElementAt(3));
            Assert.AreEqual(new DateTime(2019, 2, 24), list.ElementAt(4));
            Assert.AreEqual(new DateTime(2019, 3, 8), list.ElementAt(5));
            Assert.AreEqual(new DateTime(2019, 3, 16), list.ElementAt(6));
        }
    }
}
