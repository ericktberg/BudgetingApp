using Moq;
using System;

namespace Sunsets.Transactions.Tests.Unit.RecurringTransactionTests
{
    public class RecurringTransactionTester
    {
        public RecurringTransactionTester()
        {
            MockFrequency = new MockFrequency();
            BaseTransaction = new MockTransaction() { Value = 500 };
        }

        public DateTime StartDate { get; } = new DateTime(2019, 1, 1);

        public MockTransaction BaseTransaction { get; }

        public RecurringTransaction NewRecurringTransaction()
        {
            return new RecurringTransaction(BaseTransaction.Object, MockFrequency.Object, StartDate, null);
        }

        public MockFrequency MockFrequency { get; }
    }
}