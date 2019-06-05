using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions.Tests.Unit.FrequencyTests.MonthlyEventTests
{
    [TestClass]
    public class ElapsedEvents
    {
        [TestMethod]
        public void Should_ElapseZero()
        {
            MonthlyEvent e = new MonthlyEvent(2);

            Assert.AreEqual(0, e.ElapsedEvents(new DateTime(2019, 1, 3), new DateTime(2019, 1, 28)));
        }

        [TestMethod]
        public void Should_ElapseOnce()
        {
            MonthlyEvent e = new MonthlyEvent(2);

            Assert.AreEqual(1, e.ElapsedEvents(new DateTime(2019, 1, 1), new DateTime(2019, 1, 28)));
        }

        [TestMethod]
        public void Should_ElapseTwice()
        {
            MonthlyEvent e = new MonthlyEvent(2);

            Assert.AreEqual(2, e.ElapsedEvents(new DateTime(2019, 1, 1), new DateTime(2019, 2, 15)));
        }

        [TestMethod]
        public void Should_ElapseOnce_On_SameDay()
        {
            MonthlyEvent e = new MonthlyEvent(15);

            Assert.AreEqual(1, e.ElapsedEvents(new DateTime(2019, 1, 15), new DateTime(2019, 1, 15)));
        }

        [TestMethod]
        public void Should_ElapseTwice_Ending_ExactlyOneMonth_Apart()
        {
            MonthlyEvent e = new MonthlyEvent(15);

            Assert.AreEqual(2, e.ElapsedEvents(new DateTime(2019, 1, 15), new DateTime(2019, 2, 15)));
        }

        [TestMethod]
        public void Should_Elapse_When_Day_Is_OutOfBounds_Of_Month()
        {
            // This essentially means that we can do last of the month values by setting the 31st
            MonthlyEvent e = new MonthlyEvent(31);

            Assert.AreEqual(1, e.ElapsedEvents(new DateTime(2019, 2, 15), new DateTime(2019, 3, 15)));
        }
    }
}
