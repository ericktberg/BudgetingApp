using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Transactions.Accounts;

namespace Transactions
{
    public class AccountManager
    {
        public AccountManager()
        {
            Accounts = new List<Account>();
            Calendar = new Calendar();
        }

        private AccountManager(IList<Account> accounts, Calendar calendar)
        {
            Accounts = accounts;
            Calendar = calendar;
        }

        public IList<Account> Accounts { get; private set; }

        public Calendar Calendar { get; private set; }

        public decimal GetBalanceFromDate(Account account, DateTime date)
        {
            FinancialDay day = LatestDayWithStatement(date, account);
            Statement statement;
            if (day == null)
            {
                day = FirstDayWithStatement(date, account);

                if (day == null)
                {
                    return SumTransactionInRange(new DateTime(), date, account);
                }
                else
                {
                    statement = day.GetStatementForAccount(account);
                    return statement.Balance - SumTransactionInRange(date, day.Date - AddDay(statement, AddWhen.BeginningOfDay), account);
                }
            }
            else
            {
                statement = day.GetStatementForAccount(account);
                
                return statement.Balance + SumTransactionInRange(day.Date + AddDay(statement, AddWhen.EndOfDay), date, account);
            }
        }

        private TimeSpan AddDay(Statement statement, AddWhen when)
        {
            return statement.AddWhen == when ? TimeSpan.FromDays(1) : TimeSpan.FromDays(0);
        }

        private FinancialDay FirstDayWithStatement(DateTime startDate, Account account)
        {
            return Calendar.Days.Where(d => startDate <= d.Date).FirstOrDefault(d => d.GetStatementForAccount(account) != null);
        }

        private FinancialDay LatestDayWithStatement(DateTime endDate, Account account)
        {
            return Calendar.Days.Where(d => d.Date <= endDate).LastOrDefault(d => d.GetStatementForAccount(account) != null);
        }

        private decimal SumTransactionInRange(DateTime startDate, DateTime endDate, Account account)
        {
            return Calendar.Days.Where(d => startDate <= d.Date && d.Date <= endDate).Sum(d => d.GetTransactionsForAccount(account).Sum(a => a.GetValue(account)));
        }

        public void AddStatement(Statement statement, DateTime date)
        {
            Calendar.GetDayForDate(date).AddStatement(statement);
        }

        public void AddTransaction(Transaction transaction, DateTime date)
        {
            Calendar.GetDayForDate(date).AddTransaction(transaction);
        }

        public bool RemoveStatement(Statement statement, DateTime date)
        {
            return Calendar.GetDayForDate(date).RemoveStatement(statement);
        }

        public bool RemoveTransaction(Transaction transaction, DateTime date)
        {
            return Calendar.GetDayForDate(date).RemoveTransaction(transaction);
        }


        public decimal GetBalanceFromToday(Account account)
        {
            return GetBalanceFromDate(account, DateTime.Now);
        }

        #region IO Methods

        public static JsonSerializerSettings Settings { get; } = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto, 
        };

        public bool ToJson(Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream, new UnicodeEncoding());
            JsonSerializer serializer = JsonSerializer.Create(Settings);
            serializer.Serialize(writer, this);
            writer.Flush();

            return true;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Settings);
        }

        public static AccountManager FromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<AccountManager>(jsonString, Settings);
        }

        public static AccountManager FromJson(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            JsonSerializer serializer = JsonSerializer.Create(Settings);
            return (AccountManager) serializer.Deserialize(reader, typeof(AccountManager));
        }

        #endregion
    }
}