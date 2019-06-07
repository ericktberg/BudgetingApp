using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Sunsets.Transactions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Calendar
    {
        public event EventHandler DayCollectionChanged;

        public IEnumerable<FinancialDay> Days => DayCollection;

        public IEnumerable<RecurringTransaction> RecurringTransactions => RecurringTransactionsList;

        public void AddRecurringTransaction(RecurringTransaction transaction)
        {
            RecurringTransactionsList.Add(transaction);
        }

        [JsonProperty]
        private IList<FinancialDay> DayCollection { get; } = new List<FinancialDay>();

        private IList<RecurringTransaction> RecurringTransactionsList { get; } = new List<RecurringTransaction>();

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