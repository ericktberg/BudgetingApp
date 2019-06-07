using Moq;

namespace Sunsets.Transactions.Tests.Unit
{
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
