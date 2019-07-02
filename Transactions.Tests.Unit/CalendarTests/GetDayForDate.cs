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

            Assert.AreSame(day1, day2);
            Assert.AreEqual(1, Calendar.Days.Count());
        }

        [TestMethod]
        public void Should_Ignore_Time_Of_Day()
        {
            FinancialDay day1 = Calendar.GetDayForDate(new DateTime(2000, 1, 1, 12, 0, 0));

            FinancialDay day2 = Calendar.GetDayForDate(new DateTime(2000, 1, 1, 12, 30, 0));

            Assert.AreSame(day1, day2);
            Assert.AreEqual(1, Calendar.Days.Count());
        }

        [TestMethod]
        public void Should_AddNextDay()
        {
            FinancialDay day1 = Calendar.GetDayForDate(new DateTime(2000, 1, 1));

            Assert.IsNull(day1.NextDay);

            FinancialDay day2 = Calendar.GetDayForDate(new DateTime(2000, 1, 2));

            Assert.AreSame(day2, day1.NextDay);
        }

        [TestMethod]
        public void Should_AddPreviousDay()
        {
            FinancialDay day1 = Calendar.GetDayForDate(new DateTime(2000, 1, 1));
            
            FinancialDay day2 = Calendar.GetDayForDate(new DateTime(1999, 12, 1));

            Assert.AreSame(day2, day1.PreviousDay);
        }

        [TestMethod]
        public void Should_Replace_Previous_And_NextDays()
        {
            FinancialDay day1 = Calendar.GetDayForDate(new DateTime(2000, 1, 1));

            FinancialDay day3 = Calendar.GetDayForDate(new DateTime(2000, 1, 3));

            Assert.AreSame(day3, day1.NextDay);
            Assert.AreSame(day1, day3.PreviousDay);

            FinancialDay day2 = Calendar.GetDayForDate(new DateTime(2000, 1, 2));

            Assert.AreSame(day2, day1.NextDay);
            Assert.AreSame(day2, day3.PreviousDay);
        }

        [TestMethod]
        public void Should_OrderBy_YearFirst()
        {
            FinancialDay day2 = Calendar.GetDayForDate(new DateTime(2000, 1, 1));
            FinancialDay day1 = Calendar.GetDayForDate(new DateTime(1999, 12, 1));

            Assert.AreEqual(day1, Calendar.Days.ElementAt(0));
            Assert.AreEqual(day2, Calendar.Days.ElementAt(1));
        }

        [TestMethod]
        public void Should_OrderBy_While_Inserting()
        {
            FinancialDay day1 = Calendar.GetDayForDate(new DateTime(2000, 1, 1));
            FinancialDay day3 = Calendar.GetDayForDate(new DateTime(2000, 1, 3));
            FinancialDay day2 = Calendar.GetDayForDate(new DateTime(2000, 1, 2));

            Assert.AreEqual(day1, Calendar.Days.ElementAt(0));
            Assert.AreEqual(day2, Calendar.Days.ElementAt(1));
            Assert.AreEqual(day3, Calendar.Days.ElementAt(2));
        }
    }
}
