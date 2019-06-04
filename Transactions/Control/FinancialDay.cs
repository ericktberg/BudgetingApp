using Newtonsoft.Json;
using Sunsets.Transactions.Accounts;
using System;
using System.Collections.Generic;

namespace Sunsets.Transactions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FinancialDay
    {
        public FinancialDay(DateTime day)
        {
            Date = day;

            TransactionCollection = new List<Transaction>();
            Statements = new List<Statement>();
        }

        public event EventHandler StatementsChanged;

        public event EventHandler TransactionsChanged;

        [JsonProperty]
        public DateTime Date { get; private set; }

        [JsonProperty]
        public IList<Statement> Statements { get; private set; }

        [JsonProperty]
        public IList<Transaction> TransactionCollection { get; private set; }

        public void AddStatement(Statement statement)
        {
            Statements.Add(statement);
            OnStatementsChanged();
        }

        public bool AddTransaction(Transaction transaction)
        {
            if (TransactionCollection.Contains(transaction))
            {
                return false;
            }

            TransactionCollection.Add(transaction);
            OnTransactionsChanged();
            return true;
        }

        public bool RemoveStatement(Statement statement)
        {
            if (Statements.Remove(statement))
            {
                OnStatementsChanged();
                return true;
            }
            else { return false; }
        }

        public bool RemoveTransaction(Transaction transaction)
        {
            if (TransactionCollection.Remove(transaction))
            {
                OnTransactionsChanged();
                return true;
            }
            else { return false; }
        }

        private void OnStatementsChanged()
        {
            StatementsChanged?.Invoke(this, new EventArgs());
        }

        private void OnTransactionsChanged()
        {
            TransactionsChanged?.Invoke(this, new EventArgs());
        }
    }

    [JsonArray]
    public class StatementDictionary : Dictionary<Account, Statement>
    {
    }
}