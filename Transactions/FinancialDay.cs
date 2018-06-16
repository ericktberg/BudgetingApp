using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Transactions.Accounts;

namespace Transactions
{
    [JsonArray]
    public class StatementDictionary : Dictionary<Account, Statement>
    {

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class FinancialDay : IManageDailyTransactions
    {
        public FinancialDay(DateTime day)
        {
            Date = day;

            TransactionCollection = new List<Transaction>();
            Statements = new List<Statement>();
        }

        [JsonProperty]
        public DateTime Date { get; private set; }

        [JsonProperty]
        public IList<Statement> Statements { get; private set; }

        [JsonProperty]
        public IList<Transaction> TransactionCollection { get; private set; }

        public void AddStatement(Statement statement)
        {
            Statements.Add(statement);
        }

        public void AddTransaction(Transaction transaction)
        {
            TransactionCollection.Add(transaction);
        }

        public Statement GetStatementForAccount(Account account)
        {
            return Statements.FirstOrDefault(s => s.Account.Equals(account));
        }

        public IEnumerable<Transaction> GetTransactionsForAccount(Account account)
        {
            return TransactionCollection.Where(t => (t.AccountDepositedTo?.Equals(account) ?? false) || (t.AccountWithdrawnFrom?.Equals(account) ?? false));
        }

        public bool RemoveStatement(Statement statement)
        {
            return Statements.Remove(statement);
        }

        public bool RemoveTransaction(Transaction transaction)
        {
            return TransactionCollection.Remove(transaction);
        }
    }
}