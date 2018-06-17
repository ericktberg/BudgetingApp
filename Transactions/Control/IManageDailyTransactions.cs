using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Accounts;

namespace Transactions
{
    public interface IManageDailyTransactions
    {
        DateTime Date { get; }

        IList<Statement> Statements
        {
            get;
        }

        IList<Transaction> TransactionCollection
        {
            get;
        }

        /// <summary>
        /// Add a transaction to the day's list of transactions
        /// </summary>
        /// <param name="transaction">The transaction to add</param>
        void AddTransaction(Transaction transaction);

        /// <summary>
        /// Remvove a transaction from the day's list of transactions
        /// </summary>
        /// <param name="transaction">The transaction to remove</param>
        /// <returns>Returns true if a transaction was removed. False otherwise.</returns>
        bool RemoveTransaction(Transaction transaction);

        /// <summary>
        /// Add a statement to the day's list of statements
        /// </summary>
        /// <param name="statement">The statement to add</param>
        void AddStatement(Statement statement);

        /// <summary>
        /// Remove a statement from the day's list of statements
        /// </summary>
        /// <param name="statement">The statement to remove</param>
        /// <returns>Returns true if a statement was removed. False otherwise.</returns>
        bool RemoveStatement(Statement statement);

        /// <summary>
        /// Get a collection of transactions for the account queried
        /// </summary>
        /// <param name="account">The account the transactions apply to</param>
        /// <returns>An enumeration of transactions. Empty if there are none.</returns>
        IEnumerable<Transaction> GetTransactionsForAccount(Account account);

        /// <summary>
        /// Get a statement that applies to the account
        /// </summary>
        /// <param name="account">The account the statement is for</param>
        /// <returns>The statement received on this day. Null if there is no statement for this account.</returns>
        Statement GetStatementForAccount(Account account);
    }
}
