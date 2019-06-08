using Moq;
using System;
using System.Collections.Generic;

namespace Sunsets.Transactions.Tests.Unit
{
    public class MockRecurringTransaction : Mock<IRecurringTransaction>
    {
        public MockRecurringTransaction()
        {
            Setup(_ => _.EnumerateElementsUntilDate(It.IsAny<DateTime>()))
                .Callback((DateTime to) =>
                {
                    List<RecurringTransactionElement> list = new List<RecurringTransactionElement>();

                    foreach (var d in Dates)
                    {
                        if (d <= to)
                        {
                            list.Add(new RecurringTransactionElement(Object, d));
                        }
                    }

                    Setup(_ => _.Elements).Returns(list);
                });

            Setup(_ => _.BaseTransaction).Returns(() => BaseTransaction.Object);
        }


        public IList<DateTime> Dates { get; } = new List<DateTime>();

        public MockTransaction BaseTransaction { get; set; } = new MockTransaction() { Value = 500 };
    }

    public class MockBalance : Mock<IHaveBalance>
    {
        public MockBalance(decimal? starting, decimal? ending)
        {
            Setup(_ => _.EndingBalance).Returns(() => EndingBalance);
            Setup(_ => _.StartingBalance).Returns(() => StartingBalance);
            Setup(_ => _.StatementOnLeft()).Returns(() => HasStatementOnLeft);
            Setup(_ => _.StatementOnRight()).Returns(() => HasStatementOnRight);
            Setup(_ => _.HasStatement).Returns(() => HasStatement);

            if (starting.HasValue)
            {
                StartingBalance = starting.Value;
            }

            if (ending.HasValue)
            {
                EndingBalance = ending.Value;
            }
        }

        public decimal EndingBalance { get; set; }

        public decimal StartingBalance { get; set; }
        
        public bool HasStatementOnRight { get; set; }

        public bool HasStatementOnLeft { get; set; }
        
        public bool HasStatement { get; set; }
    }
}
