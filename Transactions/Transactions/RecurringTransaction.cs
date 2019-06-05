using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions
{

    public class RecurringTransaction
    {
        public RecurringTransaction(Transaction baseTransaction, IFrequency frequency, DateTime startDate, DateTime? endDate)
        {
            BaseTransaction = baseTransaction;
            Frequency = frequency;
            StartDate = startDate;
            EndDate = endDate;
        }

        public IFrequency Frequency { get; }

        public Transaction BaseTransaction { get; }

        public DateTime StartDate { get; }

        public DateTime? EndDate { get; }
    }
}
