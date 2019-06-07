using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions
{

    public class RecurringTransactionElement : ITransaction
    {
        public RecurringTransactionElement(RecurringTransaction parent)
        {
            Parent = parent;
        }

        public RecurringTransaction Parent { get; }

        public decimal Value => Parent.BaseTransaction.Value;
    }

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

        public decimal GetValueBetweenDates(DateTime from, DateTime to)
        {
            from = MaxDateTime(StartDate, from);

            if (EndDate.HasValue)
            {
                to = MinDateTime(EndDate.Value, to);

                if (from > EndDate.Value)
                {
                    return 0;
                }
            }

            if (to < StartDate)
            {
                return 0;
            }

            return 0; // Frequency.ElapsedEvents(from, to) * BaseTransaction.Value;
        }

        private DateTime MinDateTime(DateTime a, DateTime b)
        {
            return new DateTime(Math.Min(a.Ticks, b.Ticks));
        }

        private DateTime MaxDateTime(DateTime a, DateTime b)
        {
            return new DateTime(Math.Max(a.Ticks, b.Ticks));
        }
    }
}
