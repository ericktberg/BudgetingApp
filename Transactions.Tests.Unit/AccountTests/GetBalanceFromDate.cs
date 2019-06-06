using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Transactions.Tests.Unit.AccountTests
{

    [TestClass]
    public class GetBalanceFromDate
    {
        public Calendar Calendar => Account.Calendar;

        public Account Account { get; set; }

        public DateTime Date { get; } = new DateTime(2005, 5, 5);

        [TestInitialize]
        public void Init()
        {
            Account = new Account("Test", AccountType.Liquid);
        }

        [TestMethod]
        public void ShouldNot_AddTransactions_AfterDate()
        {
            Account.AddTransaction(new Income(400), new DateTime(2001, 1, 2));
            Account.AddTransaction(new Expense(600), new DateTime(2001, 1, 5));

            Assert.AreEqual(400, Account.GetBalanceFromDate(new DateTime(2001, 1, 3)));
        }

        [TestMethod]
        public void ShouldNot_ApplyStatements_AfterDate()
        {
            Account.AddStatement(new Statement(1000), new DateTime(2001, 1, 1));
            Account.AddStatement(new Statement(2000), new DateTime(2001, 1, 5));

            Assert.AreEqual(1000, Account.GetBalanceFromDate(new DateTime(2001, 1, 3)));
        }
        
        [TestMethod]
        public void ShouldHave_Zero_DefaultBalance()
        {
            Assert.AreEqual(0, Account.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void ShouldTake_OnlyStatement()
        {
            decimal statementBalance = 1000;

            Account.AddStatement(new Statement(statementBalance), new DateTime());

            Assert.AreEqual(statementBalance, Account.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void ShouldTake_LatestStatement_RegardlessOf_OrderAdded()
        {
            Account.AddStatement(new Statement(1000), new DateTime(1999, 1, 1));
            Account.AddStatement(new Statement(500), new DateTime(1995, 1, 1));

            Assert.AreEqual(1000, Account.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void ShouldAdd_Income_To_Balance()
        {
            Account.AddTransaction(new Income(400), new DateTime(2000, 1, 1));

            Assert.AreEqual(400, Account.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void ShouldSubtract_Expenses_From_Balance()
        {
            Account.AddTransaction(new Expense(400), new DateTime(2000, 1, 1));

            Assert.AreEqual(-400, Account.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void ShouldOverride_OldTransactions_With_NewStatements()
        {
            Account.AddTransaction(new Income(400), new DateTime(2000, 1, 1));
            Account.AddStatement(new Statement(1000), new DateTime(2001, 1, 1));

            Assert.AreEqual(1000, Account.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void ShouldAdd_NewTransactions_To_OldStatements()
        {
            Account.AddStatement(new Statement(1000), new DateTime(2001, 1, 1));
            Account.AddTransaction(new Income(400), new DateTime(2001, 1, 2));
            Account.AddTransaction(new Expense(600), new DateTime(2001, 1, 3));

            Assert.AreEqual(1000 + 400 - 600, Account.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void ShouldMaintain_Wealth_With_Transfers()
        {
            var transferAccount = new Account("Test2", AccountType.Liquid);

            Account.AddTransaction(new TransferTo(400, transferAccount), new DateTime(2000, 1, 1));

            Assert.AreEqual(400, Account.GetBalanceFromDate(Date));
            Assert.AreEqual(-400, transferAccount.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void ShouldAdd_Transactions_To_Statement_When_BeginningOfDay()
        {
            Account.AddStatement(new Statement(1000) { AddWhen = AddWhen.StartOfDay }, new DateTime(2000, 1, 1));
            Account.AddTransaction(new Income(400), new DateTime(2000, 1, 1));

            Assert.AreEqual(1400, Account.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void ShouldOverwrite_Transactions_With_Statement_When_EndOfDay()
        {
            Account.AddStatement(new Statement(1000) { AddWhen = AddWhen.EndOfDay }, new DateTime(2000, 1, 1));
            Account.AddTransaction(new Income(400), new DateTime(2000, 1, 1));

            Assert.AreEqual(1000, Account.GetBalanceFromDate(Date));
        }

        [TestMethod]
        public void ShouldUse_FutureStatements_WhenNo_Previous_Exist()
        {
            Account.AddStatement(new Statement(1000), new DateTime(2000, 1, 1));
            Account.AddTransaction(new Expense(400), new DateTime(1999, 6, 1));

            Assert.AreEqual(1400, Account.GetBalanceFromDate(new DateTime(1999, 1, 1)));
        }

        [TestMethod]
        public void ShouldOverwrite_Transactions_With_Statement_When_BeginningOfDay_And_ExtrapolatingBackwards()
        {
            Account.AddStatement(new Statement(1000) { AddWhen = AddWhen.StartOfDay }, new DateTime(2000, 1, 1));
            Account.AddTransaction(new Expense(400), new DateTime(2000, 1, 1));

            Assert.AreEqual(1000, Account.GetBalanceFromDate(new DateTime(1999, 1, 1)));
        }
    }
}
