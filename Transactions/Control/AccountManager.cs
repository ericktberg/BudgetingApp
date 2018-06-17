using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            decimal calculatedBalance = 0;
            
            foreach (FinancialDay day in Calendar.Days)
            {
                if (day.Date < date)
                {
                    Statement possibleStatement = day.GetStatementForAccount(account);
                    
                    if (possibleStatement != null)
                    {
                        calculatedBalance = possibleStatement.Balance;
                    }

                    if (possibleStatement == null || possibleStatement.AddWhen == AddWhen.BeginningOfDay)
                    {
                        IEnumerable<Transaction> transactions = day.GetTransactionsForAccount(account);

                        foreach (Transaction transaction in transactions)
                        {
                            calculatedBalance += transaction.GetValue(account);
                        }
                    }
                }
            }

            return calculatedBalance;
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