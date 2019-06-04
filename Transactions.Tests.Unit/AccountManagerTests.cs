﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using Transactions;
using Transactions.Accounts;

namespace TransactionsTests
{
    [TestClass]
    public class AccountManagerTests
    {
        public AccountManager Personal { get; } = new AccountManager();

        [TestClass]
        public class ToJson : AccountManagerTests
        {
            [TestMethod]
            public void Should_Create_From_String()
            {
                string result = Personal.ToJson();

                AccountManager duplicate = AccountManager.FromJson(result);

                Assert.IsNotNull(duplicate);
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
            public void Should_Save_And_Load_Transactions()
            {
                Account mainAccount = new Account("TestAccount", AccountType.Liquid);

                Personal.Accounts.Add(mainAccount);
                mainAccount.Deposit(new Income(500), new DateTime(2001, 2, 3));

                string json = Personal.ToJson();
                AccountManager duplicate = AccountManager.FromJson(json);

                var duplicateAccount = duplicate.Accounts.First();
                var transactions = duplicateAccount.Calendar.Days.First().TransactionCollection;

                Assert.AreEqual(1, transactions.Count());
                Assert.AreEqual(500, transactions.First().Amount);
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
        }

        [TestClass]
        public class ToJsonFile : AccountManagerTests
        {
            public static string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            public static string path = Path.Combine(folder, "test.json");

            [TestInitialize]
            public void Init()
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            [TestCleanup]
            public void Cleanup()
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            [TestMethod]
            public void Should_Create_Readable_Stream()
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Personal.ToJson(stream);

                    stream.Position = 0;
                    AccountManager duplicate = AccountManager.FromJson(stream);

                    Assert.IsNotNull(duplicate);
                }
            }

            [TestMethod]
            public void Should_Read_From_File()
            {
                using (FileStream stream = File.Create(path))
                {
                    Personal.ToJson(stream);

                    Assert.IsTrue(File.Exists(path));
                }

                using (FileStream stream = File.OpenRead(path))
                {
                    AccountManager duplicate = AccountManager.FromJson(stream);

                    Assert.IsNotNull(duplicate);
                }
            }

            [TestMethod]
            public void Should_Save_And_Load_Days_Date()
            {
                
                Account mainAccount = new Account("TestAccount", AccountType.Liquid);
                Personal.Accounts.Add(mainAccount);
                mainAccount.Calendar.GetDayForDate(new DateTime(2001, 2, 3));

                using (MemoryStream stream = new MemoryStream())
                {
                    Personal.ToJson(stream);

                    stream.Position = 0;
                    AccountManager duplicate = AccountManager.FromJson(stream);

                    Assert.AreEqual(1, duplicate.Accounts.First().Calendar.Days.Count());

                    Assert.AreEqual(Personal.Accounts.First().Calendar.Days.First().Date, duplicate.Accounts.First().Calendar.Days.First().Date);
                }
            }
        }

        
    }
}