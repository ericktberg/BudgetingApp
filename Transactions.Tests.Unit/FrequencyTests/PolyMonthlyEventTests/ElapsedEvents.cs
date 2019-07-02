using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions.Tests.Unit.FrequencyTests.PolyMonthlyEventTests
{

    [TestClass]
    public class ElapsedEvents
    {
        [TestMethod]
        public void Should_ElapseZero()
        {
            PolyMonthlyEvent e = new PolyMonthlyEvent(8, 16, 24);

            Assert.AreEqual(0, e.ElapsedEvents(new DateTime(2019, 1, 9), new DateTime(2019, 1, 15)));
        }

        [TestMethod]
        public void Should_ElapseOnce()
        {
            PolyMonthlyEvent e = new PolyMonthlyEvent(8, 16, 24);

            Assert.AreEqual(1, e.ElapsedEvents(new DateTime(2019, 1, 1), new DateTime(2019, 1, 15)));
        }

        [TestMethod]
        public void Should_ElapseTwice()
        {
            PolyMonthlyEvent e = new PolyMonthlyEvent(8, 16, 24);

            Assert.AreEqual(2, e.ElapsedEvents(new DateTime(2019, 1, 1), new DateTime(2019, 1, 18)));
        }

        [TestMethod]
        public void Should_ElapseThreeTimes()
        {
            PolyMonthlyEvent e = new PolyMonthlyEvent(8, 16, 24);

            Assert.AreEqual(3, e.ElapsedEvents(new DateTime(2019, 1, 1), new DateTime(2019, 1, 26)));
        }

        [TestMethod]
        public void Should_ElapseFiveTimes()
        {
            PolyMonthlyEvent e = new PolyMonthlyEvent(8, 16, 24);

            Assert.AreEqual(5, e.ElapsedEvents(new DateTime(2019, 1, 1), new DateTime(2019, 2, 18)));
        }

        [TestMethod]
        public void Should_ElapseSevenTimes()
        {
            PolyMonthlyEvent e = new PolyMonthlyEvent(8, 16, 24);

            Assert.AreEqual(7, e.ElapsedEvents(new DateTime(2019, 1, 9), new DateTime(2019, 3, 17)));
        }
    }
}
