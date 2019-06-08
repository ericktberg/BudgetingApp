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

        public IList<RecurringTransaction> RecurringTransactions { get; } = new List<RecurringTransaction>();

        [JsonProperty]
        public AccountType Type { get; set; }

        public void AddRecurringTransaction(IRecurringTransaction transaction)
        {
            Calendar.AddRecurringTransaction(transaction);
        }

        public void AddStatement(Statement statement, DateTime date)
        {
            Calendar.GetDayForDate(date).AddStatement(statement);
        }

        public bool AddTransaction(ITransaction transaction, DateTime date)
        {
            return Calendar.GetDayForDate(date).AddTransaction(transaction);
        }

        public decimal GetBalanceFromDate(DateTime date)
        {
            return GetDelta(Calendar.GetDayForDate(date).EndingBalance);
        }

        public virtual decimal GetDelta(decimal amount)
        {
            return amount;
        }

        public void RemoveStatement(Statement statement, DateTime date)
        {
            Calendar.GetDayForDate(date).RemoveStatement();
        }
        
        [System.Obsolete]
        private void TransferFrom(TransferFrom transfer, DateTime date)
        {
            if (AddTransaction(transfer, date))
            {
                transfer.AccountDepositedTo.TransferTo(new TransferTo(transfer.Amount, this, transfer.TransactionGuid), date);
            }
        }

        [System.Obsolete]
        private void TransferTo(TransferTo transfer, DateTime date)
        {
            if (AddTransaction(transfer, date))
            {
                transfer.AccountWithdrawnFrom.TransferFrom(new TransferFrom(transfer.Amount, this, transfer.TransactionGuid), date);
            }
        }
    }
}