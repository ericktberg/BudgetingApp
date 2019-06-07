using System;

namespace Sunsets.Transactions.Tests.Unit.CalendarTests
{
    public class CalendarTester
    {
        public Calendar NewCalendar()
        {
            return new Calendar();
        }

        public MockFrequency MockFrequency { get; } = new MockFrequency();

        public DateTime StartDate { get; } = new DateTime(2019, 1, 1);

        public RecurringTransaction NewRecurringTransaction()
        {
            return new RecurringTransaction(new MockTransaction() { Value = 500 }.Object, MockFrequency.Object, StartDate, null);
        }
    }
}
