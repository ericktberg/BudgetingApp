using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunsets.Transactions
{
    /// <summary>
    /// An event that occurs multiple times per month on specific days.
    /// </summary>
    /// <seealso cref="Sunsets.Transactions.IFrequency" />
    public class PolyMonthlyEvent : IFrequency
    {
        public PolyMonthlyEvent(params int[] days)
        {
            Days = days;

            Events = Days.Select(d => new MonthlyEvent(d));
        }

        public IEnumerable<int> Days { get; }

        private IEnumerable<MonthlyEvent> Events { get; }

        public int ElapsedEvents(DateTime startDate, DateTime endDate)
        {
            return Events.Sum(e => e.ElapsedEvents(startDate, endDate));
        }
    }
}
