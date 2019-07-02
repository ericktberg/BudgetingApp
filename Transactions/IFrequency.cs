using System;
using System.Collections.Generic;

namespace Sunsets.Transactions
{
    public interface IFrequency
    {
        IEnumerable<DateTime> ListDatesBetween(DateTime from, DateTime to);
    }
}
