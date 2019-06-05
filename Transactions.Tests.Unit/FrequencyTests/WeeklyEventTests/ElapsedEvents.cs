using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions.Tests.Unit.FrequencyTests.WeeklyEventTests
{
    [TestClass]
    public class ElapsedEvents
    {
        [TestMethod]
        public void Should_ElapseZero_When_DayOfWeek_IsNot_BetweenDays()
        {
            WeeklyEvent e = new WeeklyEvent(DayOfWeek.Tuesday);

            Assert.AreEqual(0, e.ElapsedEvents(new DateTime(2019, 6, 5), new DateTime(2019, 6, 8)));
        }

        [TestMethod]
        public void Should_ElapseOnce_When_DayOfWeek_Is_BetweenDays()
        {
            WeeklyEvent e = new WeeklyEvent(DayOfWeek.Tuesday);

            Assert.AreEqual(1, e.ElapsedEvents(new DateTime(2019, 6, 1), new DateTime(2019, 6, 8)));
        }

        [TestMethod]
        public void Should_ElapseOnce_When_DayOfWeek_Is_ExactlyDays()
        {
            WeeklyEvent e = new WeeklyEvent(DayOfWeek.Tuesday);

            Assert.AreEqual(1, e.ElapsedEvents(new DateTime(2019, 6, 4), new DateTime(2019, 6, 4)));
        }

        [TestMethod]
        public void Should_ElapseTwice_When_DayOfWeek_Is_BetweenTwoWeeks()
        {
            WeeklyEvent e = new WeeklyEvent(DayOfWeek.Tuesday);

            Assert.AreEqual(2, e.ElapsedEvents(new DateTime(2019, 6, 1), new DateTime(2019, 6, 15)));
        }
    }
}
