using Moq;
using System;

namespace Sunsets.Transactions.Tests.Unit
{
    public class MockTransaction : ITransaction
    {
        public decimal Value
        {
            get => T.Object.Value;
            set
            {
                T.Setup(_ => _.Value).Returns(value);
            }
        }

        private Mock<ITransaction> T { get; } = new Mock<ITransaction>();
    }

    public class MockBalance : IHaveBalance
    {
        public MockBalance(decimal? starting, decimal? ending)
        {
            if (starting.HasValue)
            {
                StartingBalance = starting.Value;
            }

            if (ending.HasValue)
            {
                EndingBalance = ending.Value;
            }
        }

        public decimal EndingBalance
        {
            get => B.Object.EndingBalance;
            set => B.Setup(_ => _.EndingBalance).Returns(value);
        }

        public decimal StartingBalance
        {
            get => B.Object.StartingBalance;
            set => B.Setup(_ => _.StartingBalance).Returns(value);
        }

        public bool StatementOnLeft()
        {
            return B.Object.StatementOnLeft();
        }

        public bool StatementOnRight()
        {
            return B.Object.StatementOnRight();
        }

        public void UpdateBalance()
        {
        }

        public bool HasStatementOnRight
        {
            get => B.Object.StatementOnRight();
            set => B.Setup(_ => _.StatementOnRight()).Returns(value);
        }

        public bool HasStatementOnLeft
        {
            get => B.Object.StatementOnLeft();
            set => B.Setup(_ => _.StatementOnLeft()).Returns(value);
        }

        private Mock<IHaveBalance> B { get; } = new Mock<IHaveBalance>();

        public bool HasStatement
        {
            get => B.Object.HasStatement;
            set => B.Setup(_ => _.HasStatement).Returns(value);
        }
    }
}

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