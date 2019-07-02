using Moq;

namespace Sunsets.Transactions.Tests.Unit
{
    public class MockTransaction : Mock<ITransaction>
    {
        public MockTransaction()
        {
            Setup(_ => _.Value).Returns(() => Value);
        }

        public decimal Value { get; set; }
    }
}
