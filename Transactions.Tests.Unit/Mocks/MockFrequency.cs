using Moq;
using System;
using System.Collections.Generic;

namespace Sunsets.Transactions.Tests.Unit
{
    public class MockFrequency : Mock<IFrequency>
    {
        public MockFrequency()
        {
            Setup(_ => _.ListDatesBetween(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(DateCollection);
        }
        
        public List<DateTime> DateCollection { get; } = new List<DateTime>();
    }
}
