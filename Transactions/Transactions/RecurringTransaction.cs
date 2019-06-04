using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions
{
    public class RecurringTransaction
    {
        public RecurringTransaction(Transaction baseTransaction, DateTime startDate, DateTime? endDate)
        {
            BaseTransaction = baseTransaction;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Transaction BaseTransaction { get; }

        public DateTime StartDate { get; }

        public DateTime? EndDate { get; }
    }
}
