using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Transactions
{
    public interface IManageDays
    {
        /// <summary>
        /// Get the Financial Day associate with the Date asked for. Create a new day and add that to the collection if the date isn't associated already.
        /// </summary>
        /// <param name="date">The date to find</param>
        /// <returns>The Financial day associated with the date given.</returns>
        IManageDailyTransactions GetDayForDate(DateTime date);

        /// <summary>
        /// A collection of all days that have financial Transactions in them.
        /// </summary>
        IEnumerable<IManageDailyTransactions> Days { get; }
    }
}
