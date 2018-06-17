using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Transactions.Accounts;

namespace Transactions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Calendar
    {
        public IEnumerable<FinancialDay> Days => DayCollection;

        [JsonProperty]
        private IList<FinancialDay> DayCollection { get; } = new List<FinancialDay>();

        public event EventHandler DayCollectionChanged;

        public FinancialDay GetDayForDate(DateTime date)
        {
            lock (DayCollection)
            {
                for (int i = 0; i < DayCollection.Count; i++)
                {
                    var day = DayCollection[i];

                    if (day.Date.Date.Equals(date.Date))
                    {
                        return day;
                    }
                    else if (day.Date > date)
                    {
                        DayCollection.Insert(i, new FinancialDay(date));
                        OnDayCollectionChanged();
                        return DayCollection[i];
                    }
                }
            }

            DayCollection.Add(new FinancialDay(date));
            OnDayCollectionChanged();
            return DayCollection.Last();
        }

        private void OnDayCollectionChanged()
        {
            DayCollectionChanged?.Invoke(this, new EventArgs());
        }
    }
}