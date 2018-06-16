using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Transactions.Accounts;

namespace Transactions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Calendar : IManageDays
    {
        public IEnumerable<IManageDailyTransactions> Days => DayCollection;

        [JsonProperty]
        private IList<IManageDailyTransactions> DayCollection { get; } = new List<IManageDailyTransactions>();


        public IManageDailyTransactions GetDayForDate(DateTime date)
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
                        return DayCollection[i];
                    }
                }
            }

            DayCollection.Add(new FinancialDay(date));
            return DayCollection.Last();
        }


    }
}