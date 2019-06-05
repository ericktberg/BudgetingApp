using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunsets.Transactions.Accounts
{
    /// <summary>
    /// An <see cref="Account"/> is used to monitor and keep track of a real-life account's balance.
    /// <para/>
    /// This could be a checking account, savings account, credit card balance, loan, etc.
    /// </summary>
    public class Account
    {
        public Account(string name, AccountType type) : this(name, type, new Calendar(), Guid.NewGuid())
        {
        }

        [JsonConstructor]
        public Account(string name, AccountType type, Calendar calendar, Guid guid)
        {
            Name = name;
            Type = type;
            Calendar = calendar;
            AccountGuid = guid;
        }

        [JsonProperty]
        public Guid AccountGuid { get; }

        [JsonProperty]
        public Calendar Calendar { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public AccountType Type { get; set; }

        public IList<RecurringTransaction> RecurringTransactions { get; } = new List<RecurringTransaction>();

        public void AddRecurringTransaction(RecurringTransaction transaction)
        {
            RecurringTransactions.Add(transaction);
        }

        public void AddStatement(Statement statement, DateTime date)
        {
            Calendar.GetDayForDate(date).AddStatement(statement);
        }

        public void Deposit(Income income, DateTime date)
        {
            AddTransaction(income, date);
        }

        public decimal GetBalanceFromDate(DateTime date)
        {
            FinancialDay day = LatestDayWithStatement(date);
            Statement statement;
            if (day == null)
            {
                day = FirstDayWithStatement(date);

                if (day == null)
                {
                    return SumTransactionInRange(new DateTime(), date);
                }
                else
                {
                    statement = day.Statements.LastOrDefault();
                    return statement.Balance - SumTransactionInRange(date, day.Date - AddDay(statement, AddWhen.BeginningOfDay));
                }
            }
            else
            {
                statement = day.Statements.LastOrDefault();

                return statement.Balance + SumTransactionInRange(day.Date + AddDay(statement, AddWhen.EndOfDay), date);
            }
        }

        public decimal GetBalanceFromToday()
        {
            return GetBalanceFromDate(DateTime.Now);
        }

        public virtual decimal GetDelta(decimal amount)
        {
            return amount;
        }

        public bool RemoveStatement(Statement statement, DateTime date)
        {
            return Calendar.GetDayForDate(date).RemoveStatement(statement);
        }

        public void TransferFrom(TransferFrom transfer, DateTime date)
        {
            if (AddTransaction(transfer, date))
            {
                transfer.AccountDepositedTo.TransferTo(new TransferTo(transfer.Amount, this, transfer.TransactionGuid), date);
            }
        }

        public void TransferTo(TransferTo transfer, DateTime date)
        {
            if (AddTransaction(transfer, date))
            {
                transfer.AccountWithdrawnFrom.TransferFrom(new TransferFrom(transfer.Amount, this, transfer.TransactionGuid), date);
            }
        }

        public void Withdraw(Expense expense, DateTime date)
        {
            AddTransaction(expense, date);
        }

        private TimeSpan AddDay(Statement statement, AddWhen when)
        {
            return statement.AddWhen == when ? TimeSpan.FromDays(1) : TimeSpan.FromDays(0);
        }

        private bool AddTransaction(Transaction transaction, DateTime date)
        {
            return Calendar.GetDayForDate(date).AddTransaction(transaction);
        }

        private FinancialDay FirstDayWithStatement(DateTime startDate)
        {
            return Calendar.Days.Where(d => startDate <= d.Date).FirstOrDefault(d => d.Statements.LastOrDefault() != null);
        }

        private FinancialDay LatestDayWithStatement(DateTime endDate)
        {
            return Calendar.Days.Where(d => d.Date <= endDate).LastOrDefault(d => d.Statements.LastOrDefault() != null);
        }

        private bool RemoveTransaction(Transaction transaction, DateTime date)
        {
            return Calendar.GetDayForDate(date).RemoveTransaction(transaction);
        }
    
        private decimal SumTransactionInRange(DateTime startDate, DateTime endDate)
        {
            decimal summedTransactions = Calendar.Days
                .Where(d =>
                    startDate <= d.Date && d.Date <= endDate
                )
                .Sum(d =>
                    d.TransactionCollection.Sum(a => GetDelta(a.Value))
                );

            decimal summedRecurring = RecurringTransactions
                .Sum(t =>
                {


                    return t.Frequency.ElapsedEvents(MaxDateTime(t.StartDate, startDate), MinDateTime(t.EndDate ?? DateTime.MaxValue, endDate)) * GetDelta(t.BaseTransaction.Value);
                });

            return summedTransactions + summedRecurring;
        }

        private bool Between(DateTime compareDate, DateTime startDate, DateTime endDate)
        {
            return startDate <= compareDate && compareDate <= endDate;
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