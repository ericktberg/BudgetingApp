using System;

namespace Sunsets.Transactions.Tests.Unit.CalendarTests
{
    public class CalendarTester
    {
        public Calendar NewCalendar()
        {
            return new Calendar();
        }

        public MockRecurringTransaction MockRecurring { get; } = new MockRecurringTransaction();
        
        public DateTime StartDate { get; } = new DateTime(2019, 1, 1);
    }
}
