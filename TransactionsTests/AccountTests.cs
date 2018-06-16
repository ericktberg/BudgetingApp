using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Transactions;
using Transactions.Accounts;

namespace TransactionsTests
{
    [TestClass]
    public class AccountTests
    {
        public Account Account { get; set; } = new Account("Test");
    }
}
