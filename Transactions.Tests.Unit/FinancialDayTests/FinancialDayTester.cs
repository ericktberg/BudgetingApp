using System;
using Sunsets.Transactions;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Transactions.Tests.Unit.FinancialDayTests
{
    public class FinancialDayTester
    {
        public FinancialDay Day { get; } = new FinancialDay(DateTime.Now);
    }
}
