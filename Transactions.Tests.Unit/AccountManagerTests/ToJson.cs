using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions.Accounts;
using System;
using System.Linq;

namespace Sunsets.Transactions.Tests.Unit.AccountManagerTests
{
    [TestClass]
    public class ToJson
    {
        public AccountManager Personal => Tester.Personal;

        public AccountManagerTester Tester { get; } = new AccountManagerTester();

        [TestMethod]
        public void Should_Create_From_String()
        {
            string result = Personal.ToJson();

            AccountManager duplicate = AccountManager.FromJson(result);

            Assert.IsNotNull(duplicate);
        }

        [TestMethod]
        public void Should_Save_And_Load_Complex_Balances()
        {
            Account checkingAccount = new Account("Checking", AccountType.Liquid);
            Account debtAccount = new DebtAccount("Debt");
            Account savingsAccount = new Account("Savings", AccountType.Liquid);

            Personal.Accounts.Add(debtAccount);
            Personal.Accounts.Add(savingsAccount);
            Personal.Accounts.Add(checkingAccount);

            debtAccount.AddStatement(new Statement(1000), new DateTime(2000, 1, 1));
            checkingAccount.AddStatement(new Statement(2000), new DateTime(2000, 1, 1));
            savingsAccount.AddStatement(new Statement(5000), new DateTime(2000, 1, 1));

            debtAccount.TransferTo(new TransferTo(400, checkingAccount), new DateTime(2000, 1, 5));
            checkingAccount.Deposit(new Income(1000), new DateTime(2000, 1, 3));
            debtAccount.Withdraw(new Expense(200), new DateTime(2000, 1, 6));
            checkingAccount.TransferFrom(new TransferFrom(300, savingsAccount), new DateTime(2000, 1, 7));

            decimal debtBalance = debtAccount.GetBalanceFromToday();
            decimal checkingBalance = checkingAccount.GetBalanceFromToday();
            decimal savingsBalance = savingsAccount.GetBalanceFromToday();

            string json = Personal.ToJson();
            AccountManager duplicate = AccountManager.FromJson(json);

            Assert.AreEqual(debtBalance, debtAccount.GetBalanceFromToday());
            Assert.AreEqual(checkingBalance, checkingAccount.GetBalanceFromToday());
            Assert.AreEqual(savingsBalance, savingsAccount.GetBalanceFromToday());
        }

        [TestMethod]
        public void Should_Save_And_Load_Days_Date()
        {
            var account = new Account("TestAccount", AccountType.Liquid);
            Personal.Accounts.Add(account);
            FinancialDay day = account.Calendar.GetDayForDate(new DateTime(2001, 2, 3));

            string json = Personal.ToJson();
            AccountManager duplicate = AccountManager.FromJson(json);

            var duplicateDays = duplicate.Accounts.First().Calendar.Days;

            Assert.AreEqual(1, duplicateDays.Count());
            Assert.AreEqual(account.Calendar.Days.First().Date, duplicateDays.First().Date);
        }

        [TestMethod]
        public void Should_Save_And_Load_Statements()
        {
            Account mainAccount = new Account("TestAccount", AccountType.Liquid);

            Personal.Accounts.Add(mainAccount);
            mainAccount.AddStatement(new Statement(1000), new DateTime(2001, 2, 3));

            string json = Personal.ToJson();
            AccountManager duplicate = AccountManager.FromJson(json);

            var duplicateDay = duplicate.Accounts.First().Calendar.Days.First();
            var statement = duplicateDay.Statements.First();

            Assert.IsNotNull(statement);
            Assert.AreEqual(1000, statement.Balance);
        }

        [TestMethod]
        public void Should_Save_And_Load_Transactions()
        {
            Account mainAccount = new Account("TestAccount", AccountType.Liquid);

            Personal.Accounts.Add(mainAccount);
            mainAccount.Deposit(new Income(500), new DateTime(2001, 2, 3));

            string json = Personal.ToJson();
            AccountManager duplicate = AccountManager.FromJson(json);

            var duplicateAccount = duplicate.Accounts.First();
            var Transactions = duplicateAccount.Calendar.Days.First().TransactionCollection;

            Assert.AreEqual(1, Transactions.Count());
            Assert.AreEqual(500, Transactions.First().Amount);
        }

        [TestMethod]
        public void Should_Save_Load_Accounts()
        {
            Personal.Accounts.Add(new Account("TestAccount", AccountType.Liquid));

            string json = Personal.ToJson();
            AccountManager duplicate = AccountManager.FromJson(json);

            Assert.AreEqual(1, duplicate.Accounts.Count);
            Assert.AreEqual("TestAccount", duplicate.Accounts[0].Name);
        }
    }
}