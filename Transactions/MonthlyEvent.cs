using System;

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
            DateTime nextDate = startDate;
            int elapsedCount = 0;
            bool stillGoing = true;

            while (stillGoing)
            {
                int startMonth = nextDate.Month;
                int startYear = nextDate.Year;
                
                DateTime nextMonth = new DateTime(startYear, startMonth + 1, 1);
                DateTime lastDayOfMonth = nextMonth.AddDays(-1);

                if (lastDayOfMonth > endDate)
                {
                    lastDayOfMonth = endDate;
                    stillGoing = false;
                }

                int dayToCheck = Math.Min(DayOfMonth, DateTime.DaysInMonth(startYear, startMonth));

                if (nextDate.Day <= dayToCheck && dayToCheck <= lastDayOfMonth.Day)
                {
                    elapsedCount++;
                }

                nextDate = nextMonth;
            }

            return elapsedCount;
        }
    }
}
