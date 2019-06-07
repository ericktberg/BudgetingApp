using System;

namespace Sunsets.Transactions.Tests.Unit.FinancialDayTests
{
    public class FinancialDayTester
    {
        public FinancialDay Day { get; } = new FinancialDay(DateTime.Now);

        public FinancialDay GetToday()
        {
            return new FinancialDay(DateTime.Now);
        }

        public FinancialDay GetYesterday()
        {
            return new FinancialDay(DateTime.Now.AddDays(-1));
        }

        public FinancialDay GetTomorrow()
        {
            return new FinancialDay(DateTime.Now.AddDays(1));
        }
    }
}