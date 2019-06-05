using System;

namespace Sunsets.Transactions
{
    public interface IFrequency
    {
        int ElapsedEvents(DateTime startDate, DateTime endDate);
    }
}
