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

        [JsonProperty]
        private IList<FinancialDay> DayCollection { get; } = new List<FinancialDay>();

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
                    
                    if (index > 0)
                    {
                        var previous = DayCollection[index - 1];

                        previous.NextDay = returnDay;
                        returnDay.PreviousDay = previous;
                    }

                    if (index < DayCollection.Count)
                    {
                        var next = DayCollection[index];

                        next.PreviousDay = returnDay;
                        returnDay.NextDay = next;
                    }

                    DayCollection.Insert(index, returnDay);
                    OnDayCollectionChanged();
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

        private void OnDayCollectionChanged()
        {
            DayCollectionChanged?.Invoke(this, new EventArgs());
        }
    }
}