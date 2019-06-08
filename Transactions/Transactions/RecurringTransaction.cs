using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions
{

    public class RecurringTransactionElement : ITransaction
    {
        public RecurringTransactionElement(IRecurringTransaction parent, DateTime date)
        {
            Parent = parent;
            Date = date;
        }

        public DateTime Date { get; }

        public IRecurringTransaction Parent { get; }

        public decimal Value => Parent.BaseTransaction.Value;
    }

    public interface IRecurringTransaction
    {
        IEnumerable<RecurringTransactionElement> Elements { get; }

        ITransaction BaseTransaction { get; }

        void EnumerateElementsUntilDate(DateTime to);
    }

    public class RecurringTransaction : IRecurringTransaction
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
            EnumerateElementsUntilDate(to);

            return Elements
                .Where(e => e.Date.Date >= from.Date && e.Date.Date <= to.Date)
                .Sum(e => e.Value);
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
