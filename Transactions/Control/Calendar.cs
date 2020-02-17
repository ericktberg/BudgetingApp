using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunsets.Transactions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Calendar
    {
        public event EventHandler DayCollectionChanged;

        public IEnumerable<FinancialDay> Days => DayCollection;

        public IEnumerable<IRecurringTransaction> RecurringTransactions => RecurringTransactionsList;

        [System.Obsolete]
        public void AddRecurringTransaction(IRecurringTransaction transaction)
        {
            /* TODO:
             * When should we enumerate elements? When do we add them to the calendar?
             * 
             * It's not enough to add them when we add the transaction in. It's a kludge, and it shows that we can at least get it to work, but it isn't great.
             * 
             * TODO: #2
             * How does a recurring transaction handle a Transfer? It might have to be:
             *  1. Identify a transfer as a composite object made of 2 accounts and 1 Amount. The value is inverted between the accounts
             *  2. Transactions will need to be rethought completely...
             */ 
            RecurringTransactionsList.Add(transaction);

            if (DayCollection.Any())
            {
                transaction.EnumerateElementsUntilDate(DayCollection.Last().Date);
            }

            foreach (var el in transaction.Elements)
            {
                var day = GetDayForDate(el.Date);
                day.AddTransaction(el);
            }
        }

        [JsonProperty]
        private IList<FinancialDay> DayCollection { get; } = new List<FinancialDay>();

        private IList<IRecurringTransaction> RecurringTransactionsList { get; } = new List<IRecurringTransaction>();

        public FinancialDay GetDayForDate(DateTime date)
        {
            FinancialDay returnDay;

            lock (DayCollection)
            {
                var existing = FindDayForDate(date, out int index);

                if (existing != null)
                {
                    returnDay = existing;
                }
                else
                {
                    returnDay = new FinancialDay(date);

                    InsertDay(index, ref returnDay);
                }
            }

            return returnDay;
        }

        private FinancialDay FindDayForDate(DateTime date, out int index)
        {
            for (index = 0; index < DayCollection.Count; index++)
            {
                var day = DayCollection[index];

                if (day.Date.Date.Equals(date.Date))
                {
                    return day;
                }
                else if (day.Date > date)
                {
                    return null;
                }
            }

            return null;
        }

        private void InsertDay(int index, ref FinancialDay day)
        {
            if (index > 0)
            {
                var previous = DayCollection[index - 1];

                previous.NextDay = day;
                day.PreviousDay = previous;
            }

            if (index < DayCollection.Count)
            {
                var next = DayCollection[index];

                next.PreviousDay = day;
                day.NextDay = next;
            }

            DayCollection.Insert(index, day);
            OnDayCollectionChanged();
        }

        private void OnDayCollectionChanged()
        {
            DayCollectionChanged?.Invoke(this, new EventArgs());
        }
    }
}