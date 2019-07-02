using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunsets.Transactions
{
    public interface IHaveBalance
    {
        decimal EndingBalance { get; }

        bool HasStatement { get; }

        decimal StartingBalance { get; }

        bool StatementOnLeft();

        bool StatementOnRight();

        void UpdateBalance();
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class FinancialDay : IHaveBalance
    {
        private IHaveBalance _nextDay;
        private IHaveBalance _previousDay;

        public FinancialDay(DateTime day)
        {
            Date = day;

            TransactionCollection = new List<ITransaction>();
        }

        public event EventHandler BalanceChanged;

        public event EventHandler StatementsChanged;

        public event EventHandler TransactionsChanged;

        [JsonProperty]
        public DateTime Date { get; private set; }

        public decimal EndingBalance { get; private set; }

        public bool HasStatement => Statement != null;

        public IHaveBalance NextDay
        {
            get => _nextDay;
            set
            {
                _nextDay = value;
                OnBalanceChanged();
            }
        }

        public IHaveBalance PreviousDay
        {
            get => _previousDay;
            set
            {
                _previousDay = value;
                OnBalanceChanged();
            }
        }

        public decimal StartingBalance { get; private set; }

        [JsonProperty]
        public Statement Statement { get; private set; }

        [JsonProperty]
        public IList<ITransaction> TransactionCollection { get; private set; }

        public void AddStatement(Statement statement)
        {
            Statement = statement;
            OnStatementsChanged();
        }

        public bool AddTransaction(ITransaction transaction)
        {
            if (TransactionCollection.Contains(transaction))
            {
                return false;
            }

            TransactionCollection.Add(transaction);
            OnTransactionsChanged();
            return true;
        }

        public void RemoveStatement()
        {
            Statement = null;
            OnStatementsChanged();
        }

        public bool RemoveTransaction(ITransaction transaction)
        {
            if (TransactionCollection.Remove(transaction))
            {
                OnTransactionsChanged();
                return true;
            }
            else { return false; }
        }

        public bool StatementOnLeft()
        {
            if (Statement != null && Statement.AddWhen == AddWhen.StartOfDay)
            {
                return true;
            }
            else if (PreviousDay == null)
            {
                return false;
            }
            else if (PreviousDay.HasStatement)
            {
                return true;
            }
            else
            {
                return PreviousDay.StatementOnLeft();
            }
        }

        public bool StatementOnRight()
        {
            if (Statement != null && Statement.AddWhen == AddWhen.EndOfDay)
            {
                return true;
            }
            else if (NextDay == null)
            {
                return false;
            }
            else if (NextDay.HasStatement)
            {
                return true;
            }
            else
            {
                return NextDay.StatementOnRight();
            }
        }

        public void UpdateBalance()
        {
            if (!StatementOnLeft() && StatementOnRight())
            {
                UpdateBalanceFromRight();
            }
            else
            {
                UpdateBalanceFromLeft();
            }
        }

        private void UpdateBalanceFromLeft()
        {
            if (Statement != null && Statement.AddWhen == AddWhen.StartOfDay)
            {
                StartingBalance = Statement.Balance;
            }
            else if (PreviousDay != null)
            {
                PreviousDay.UpdateBalance();
                StartingBalance = PreviousDay.EndingBalance;
            }
            else
            {
                StartingBalance = 0;
            }

            if (Statement != null && Statement.AddWhen == AddWhen.EndOfDay)
            {
                EndingBalance = Statement.Balance;
            }
            else
            {
                EndingBalance = StartingBalance + TransactionCollection.Sum(_ => _.Value);
            }
        }

        private void UpdateBalanceFromRight()
        {
            if (Statement != null && Statement.AddWhen == AddWhen.EndOfDay)
            {
                EndingBalance = Statement.Balance;
            }
            else if (NextDay != null)
            {
                NextDay.UpdateBalance();
                EndingBalance = NextDay.StartingBalance;
            }
            else
            {
                throw new InvalidOperationException();
            }

            if (Statement != null && Statement.AddWhen == AddWhen.StartOfDay)
            {
                StartingBalance = Statement.Balance;
            }
            else
            {
                StartingBalance = EndingBalance - TransactionCollection.Sum(_ => _.Value);
            }
        }

        private void OnBalanceChanged()
        {
            UpdateBalance();
            BalanceChanged?.Invoke(this, new EventArgs());
        }

        private void OnStatementsChanged()
        {
            StatementsChanged?.Invoke(this, new EventArgs());
            OnBalanceChanged();
        }

        private void OnTransactionsChanged()
        {
            TransactionsChanged?.Invoke(this, new EventArgs());
            OnBalanceChanged();
        }
    }
}