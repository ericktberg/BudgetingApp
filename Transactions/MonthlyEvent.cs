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
            DateTime currentDate = startDate;
            int elapsedCount = 0;
            bool stillGoing = true;

            while (stillGoing)
            {
                int currentMonth = currentDate.Month;
                int currentYear = currentDate.Year;

                int nextMonthNumber;
                int nextYearNumber;

                if (currentMonth < 12)
                {
                    nextMonthNumber = currentMonth + 1;
                    nextYearNumber = currentYear;
                }
                else
                {
                    nextMonthNumber = 1;
                    nextYearNumber = currentYear + 1;
                }


                DateTime nextMonthDate = new DateTime(nextYearNumber, nextMonthNumber, 1);
                DateTime lastDayOfMonthDate = nextMonthDate.AddDays(-1);

                if (lastDayOfMonthDate > endDate)
                {
                    lastDayOfMonthDate = endDate;
                    stillGoing = false;
                }

                int dayToCheck = Math.Min(DayOfMonth, DateTime.DaysInMonth(currentYear, currentMonth));

                if (currentDate.Day <= dayToCheck && dayToCheck <= lastDayOfMonthDate.Day)
                {
                    elapsedCount++;
                }

                currentDate = nextMonthDate;
            }

            return elapsedCount;
        }
    }
}
