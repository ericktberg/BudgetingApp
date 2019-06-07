using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions
{

    public class RecurringTransactionElement : ITransaction
    {
        public RecurringTransactionElement(RecurringTransaction parent, DateTime date)
        {
            Parent = parent;
            Date = date;
        }

        public DateTime Date { get; }

        public RecurringTransaction Parent { get; }

        public decimal Value => Parent.BaseTransaction.Value;
    }

    public interface IRecurringTransaction
    {
        IEnumerable<RecurringTransactionElement> Elements { get; }

        void EnumerateElementsUntilDate(DateTime to);
    }

    public class RecurringTransaction
    {
        public RecurringTransaction(ITransaction baseTransaction, IFrequency frequency, DateTime startDate, DateTime? endDate)
        {
            BaseTransaction = baseTransaction;
            Frequency = frequency;
            StartDate = startDate;
            EndDate = endDate;
        }

        private IList<RecurringTransactionElement> BackingElements { get; } = new List<RecurringTransactionElement>();

        public IFrequency Frequency { get; }

        public ITransaction BaseTransaction { get; }

        public DateTime StartDate { get; }

        public DateTime? EndDate { get; }

        public IEnumerable<RecurringTransactionElement> Elements => BackingElements;

        public void EnumerateElementsUntilDate(DateTime to)
        {
            if (to.Date < StartDate.Date) return;

            var dates = Frequency.ListDatesBetween(StartDate, to);

            foreach (var date in dates)
            {
                if (date.Date >= StartDate.Date && 
                    (!EndDate.HasValue || date.Date <= EndDate.Value.Date) &&
                    !BackingElements.Any(e => e.Date.Date == date.Date))
                {
                    BackingElements.Add(new RecurringTransactionElement(this, date));
                }
            }
        }
        
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

            return Frequency.ListDatesBetween(from, to).Count() * BaseTransaction.Value;
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
