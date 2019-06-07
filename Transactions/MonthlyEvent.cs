using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunsets.Transactions
{
    /// <summary>
    /// An event that occurs monthly on a specific date.
    /// </summary>
    /// <seealso cref="Sunsets.Transactions.IFrequency" />
    public class MonthlyEvent : IFrequency
    {
        public MonthlyEvent(int dayOfMonth)
        {
            DayOfMonth = dayOfMonth;
        }

        public int DayOfMonth { get; }
        
        public int ElapsedEvents(DateTime startDate, DateTime endDate)
        {
            return ListDatesBetween(startDate, endDate).Count();
        }

        public IEnumerable<DateTime> ListDatesBetween(DateTime from, DateTime to)
        {
            var list = new List<DateTime>();
            DateTime currentDate = from;
            bool stillGoing = true;

            while (stillGoing)
            {
                int nextMonthNumber;
                int nextYearNumber;

                if (currentDate.Month < 12)
                {
                    nextMonthNumber = currentDate.Month + 1;
                    nextYearNumber = currentDate.Year;
                }
                else
                {
                    nextMonthNumber = 1;
                    nextYearNumber = currentDate.Year + 1;
                }

                DateTime nextMonthDate = new DateTime(nextYearNumber, nextMonthNumber, 1);
                DateTime lastDayOfMonthDate = nextMonthDate.AddDays(-1);

                if (lastDayOfMonthDate > to)
                {
                    lastDayOfMonthDate = to;
                    stillGoing = false;
                }

                int dayToCheck = Math.Min(DayOfMonth, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

                if (currentDate.Day <= dayToCheck && dayToCheck <= lastDayOfMonthDate.Day)
                {
                    list.Add(new DateTime(currentDate.Year, currentDate.Month, dayToCheck));
                }

                currentDate = nextMonthDate;
            }

            return list;
        }
    }
}
