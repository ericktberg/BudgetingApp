using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunsets.Transactions
{
    /// <summary>
    /// An event that occurs once a week on a specific day of the week.
    /// </summary>
    /// <seealso cref="Sunsets.Transactions.IFrequency"/>
    public class WeeklyEvent : IFrequency
    {
        public WeeklyEvent(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
        }

        public DayOfWeek DayOfWeek { get; }

        public int ElapsedEvents(DateTime startDate, DateTime endDate)
        {
            return ListDatesBetween(startDate, endDate).Count();
        }

        public IEnumerable<DateTime> ListDatesBetween(DateTime from, DateTime to)
        {
            var list = new List<DateTime>();

            DateTime nextDate = CoerceWeekday(from);

            while (nextDate <= to)
            {
                if (nextDate >= from)
                {
                    list.Add(nextDate);
                }

                nextDate = nextDate.AddDays(7);
            }

            return list;
        }

        private DateTime CoerceWeekday(DateTime date)
        {
            int difference = (int) DayOfWeek - (int) date.DayOfWeek;

            if (difference < 0)
            {
                difference += 7;
            }

            return date.AddDays(difference);
        }
    }
}