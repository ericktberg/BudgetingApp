using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunsets.Transactions;

namespace Sunsets.Transactions.Tests.Unit.CalendarTests
{
    [TestClass]
    public class GetDayForDate
    {
        public Calendar Calendar = new Calendar();

        [TestMethod]
        public void Should_Add_Day_If_Not_Present()
        {
            Assert.AreEqual(0, Calendar.Days.Count());

            Assert.IsNotNull(Calendar.GetDayForDate(new DateTime(2000, 1, 1)));

            Assert.AreEqual(1, Calendar.Days.Count());
        }

        [TestMethod]
        public void Should_Not_Add_Day_If_It_Exists_Already()
        {
            FinancialDay day1 = Calendar.GetDayForDate(new DateTime(2000, 1, 1));

            FinancialDay day2 = Calendar.GetDayForDate(new DateTime(2000, 1, 1));

            Assert.ReferenceEquals(day1, day2);
            Assert.AreEqual(1, Calendar.Days.Count());
        }

        [TestMethod]
        public void Should_Ignore_Time_Of_Day()
        {
            FinancialDay day1 = Calendar.GetDayForDate(new DateTime(2000, 1, 1, 12, 0, 0));

            FinancialDay day2 = Calendar.GetDayForDate(new DateTime(2000, 1, 1, 12, 30, 0));

            Assert.ReferenceEquals(day1, day2);
            Assert.AreEqual(1, Calendar.Days.Count());
        }
    }
}
