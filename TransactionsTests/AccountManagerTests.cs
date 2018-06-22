using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                Personal.Accounts.Add(new Account("TestAccount"));

                string json = Personal.ToJson();
                AccountManager duplicate = AccountManager.FromJson(json);

                Assert.AreEqual(1, duplicate.Accounts.Count);
                Assert.AreEqual("TestAccount", duplicate.Accounts[0].Name);
            }

            [TestMethod]
            public void Should_Save_And_Load_Days_Date()
            {
                FinancialDay day = Personal.Calendar.GetDayForDate(new DateTime(2001, 2, 3));

                string json = Personal.ToJson();
                AccountManager duplicate = AccountManager.FromJson(json);

                Assert.AreEqual(1, duplicate.Calendar.Days.Count());
                Assert.AreEqual(Personal.Calendar.Days.First().Date, duplicate.Calendar.Days.First().Date);
            }

            [TestMethod]
            public void Should_Save_And_Load_Transactions()
            {
                Account mainAccount = new Account("TestAccount");

                Personal.Accounts.Add(mainAccount);
                Personal.AddTransaction(new Income(500, mainAccount), new DateTime(2001, 2, 3));

                string json = Personal.ToJson();
                AccountManager duplicate = AccountManager.FromJson(json);

                var duplicateDay = duplicate.Calendar.Days.First();
                var transactions = duplicateDay.GetTransactionsForAccount(mainAccount);

                Assert.AreEqual(1, transactions.Count());
                Assert.AreEqual(500, transactions.First().Amount);
            }

            [TestMethod]
            public void Should_Save_And_Load_Statements()
            {
                Account mainAccount = new Account("TestAccount");

                Personal.Accounts.Add(mainAccount);
                Personal.AddStatement(new Statement(1000, mainAccount), new DateTime(2001, 2, 3));

                string json = Personal.ToJson();
                AccountManager duplicate = AccountManager.FromJson(json);

                var duplicateDay = duplicate.Calendar.Days.First();
                var statement = duplicateDay.GetStatementForAccount(mainAccount);

                Assert.IsNotNull(statement);
                Assert.AreEqual(1000, statement.Balance);
            }

            [TestMethod]
            public void Should_Save_And_Load_Complex_Balances()
            {
                Account checkingAccount = new Account("Checking");
                Account debtAccount = new DebtAccount("Debt");

                Personal.Accounts.Add(checkingAccount);
                Personal.Accounts.Add(debtAccount);

                Personal.AddStatement(new Statement(1000, debtAccount), new DateTime(2000, 1, 1));
                Personal.AddStatement(new Statement(2000, checkingAccount), new DateTime(2000, 1, 1));

                Personal.AddTransaction(new Transfer(400, checkingAccount, debtAccount), new DateTime(2000, 1, 5));
                Personal.AddTransaction(new Expense(200, debtAccount), new DateTime(2000, 1, 6));

                decimal debtBalance = Personal.GetBalanceFromToday(debtAccount);
                decimal checkingBalance = Personal.GetBalanceFromToday(checkingAccount);

                string json = Personal.ToJson();
                AccountManager duplicate = AccountManager.FromJson(json);

                Assert.AreEqual(debtBalance, duplicate.GetBalanceFromToday(debtAccount));
                Assert.AreEqual(checkingBalance, duplicate.GetBalanceFromToday(checkingAccount));
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
                Personal.Calendar.GetDayForDate(new DateTime(2001, 2, 3));

                using (MemoryStream stream = new MemoryStream())
                {
                    Personal.ToJson(stream);

                    stream.Position = 0;
                    AccountManager duplicate = AccountManager.FromJson(stream);

                    Assert.AreEqual(1, duplicate.Calendar.Days.Count());

                    Assert.AreEqual(Personal.Calendar.Days.First().Date, duplicate.Calendar.Days.First().Date);
                }
            }
        }

        [TestClass]
        public class GetBalanceFromToday : AccountManagerTests
        {
            public Calendar Calendar { get; set; }
            public Account Account { get; set; }

            [TestInitialize]
            public void Init()
            {
                Personal.Accounts.Add(new Account("Test"));

                Calendar = Personal.Calendar;
                Account = Personal.Accounts[0];
            }

            [TestMethod]
            public void Should_Have_Zero_Default_Balance()
            {
                Assert.AreEqual(0, Personal.GetBalanceFromToday(Account));
            }

            [TestMethod]
            public void Should_Take_Only_Statement()
            {
                decimal statementBalance = 1000;

                Personal.AddStatement(new Statement(statementBalance, Account), new DateTime());

                Assert.AreEqual(statementBalance, Personal.GetBalanceFromToday(Account));
            }

            [TestMethod]
            public void Should_Take_Latest_Statement_Regardless_Of_Order_Added()
            {
                Personal.AddStatement(new Statement(1000, Account), new DateTime(1999, 1, 1));
                Personal.AddStatement(new Statement(500, Account), new DateTime(1995, 1, 1));

                Assert.AreEqual(1000, Personal.GetBalanceFromToday(Account));
            }

            [TestMethod]
            public void Should_Add_Income_To_Balance()
            {
                Personal.AddTransaction(new Income(400, Account), new DateTime(2000, 1, 1));

                Assert.AreEqual(400, Personal.GetBalanceFromToday(Account));
            }

            [TestMethod]
            public void Should_Subtract_Expenses_From_Balance()
            {
                Personal.AddTransaction(new Expense(400, Account), new DateTime(2000, 1, 1));

                Assert.AreEqual(-400, Personal.GetBalanceFromToday(Account));
            }

            [TestMethod]
            public void Should_Override_Old_Transactions_With_New_Statements()
            {
                Personal.AddTransaction(new Income(400, Account), new DateTime(2000, 1, 1));
                Personal.AddStatement(new Statement(1000, Account), new DateTime(2001, 1, 1));

                Assert.AreEqual(1000, Personal.GetBalanceFromToday(Account));
            }

            [TestMethod]
            public void Should_Add_New_Transactions_To_Old_Statements()
            {
                Personal.AddStatement(new Statement(1000, Account), new DateTime(2001, 1, 1));
                Personal.AddTransaction(new Income(400, Account), new DateTime(2001, 1, 2));
                Personal.AddTransaction(new Expense(600, Account), new DateTime(2001, 1, 3));

                Assert.AreEqual(1000 + 400 - 600, Personal.GetBalanceFromToday(Account));
            }

            [TestMethod]
            public void Should_Maintain_Wealth_With_Transfers()
            {
                var transferAccount = new Account("Test2");

                Personal.AddTransaction(new Transfer(400, Account, transferAccount), new DateTime(2000, 1, 1));

                Assert.AreEqual(-400, Personal.GetBalanceFromToday(Account));
                Assert.AreEqual(400, Personal.GetBalanceFromToday(transferAccount));
            }

            [TestMethod]
            public void Should_Add_Transactions_To_Statement_When_BeginningOfDay()
            {
                Personal.AddStatement(new Statement(1000, Account) { AddWhen = AddWhen.BeginningOfDay }, new DateTime(2000, 1, 1));
                Personal.AddTransaction(new Income(400, Account), new DateTime(2000, 1, 1));

                Assert.AreEqual(1400, Personal.GetBalanceFromToday(Account));
            }

            [TestMethod]
            public void Should_Overwrite_Transactions_With_Statement_When_EndOfDay()
            {
                Personal.AddStatement(new Statement(1000, Account) { AddWhen = AddWhen.EndOfDay }, new DateTime(2000, 1, 1));
                Personal.AddTransaction(new Income(400, Account), new DateTime(2000, 1, 1));

                Assert.AreEqual(1000, Personal.GetBalanceFromToday(Account));
            }

            [TestMethod]
            public void Should_Use_Future_Statements_When_No_Previous_Exist()
            {
                Personal.AddStatement(new Statement(1000, Account), new DateTime(2000, 1, 1));
                Personal.AddTransaction(new Expense(400, Account), new DateTime(1999, 6, 1));

                Assert.AreEqual(1400, Personal.GetBalanceFromDate(Account, new DateTime(1999, 1, 1)));
            }

            [TestMethod]
            public void Should_Overwrite_Transactions_With_Statement_When_Beginning_Of_Day_And_Extrapolating_Backwards()
            {
                Personal.AddStatement(new Statement(1000, Account) { AddWhen = AddWhen.BeginningOfDay }, new DateTime(2000, 1, 1));
                Personal.AddTransaction(new Expense(400, Account), new DateTime(2000, 1, 1));

                Assert.AreEqual(1000, Personal.GetBalanceFromDate(Account, new DateTime(1999, 1, 1)));
            }
        }

        [TestClass]
        public class DebtInteraction : AccountManagerTests
        {
            public Calendar Calendar { get; set; }
            public Account Account { get; set; }

            [TestInitialize]
            public void Init()
            {
                Personal.Accounts.Add(new DebtAccount("Test"));

                Calendar = Personal.Calendar;
                Account = Personal.Accounts[0];
            }

            [TestMethod]
            public void Should_Decrease_Debt_Balance_From_Income()
            {
                Personal.AddTransaction(new Income(400, Account), new DateTime(2000, 1, 1));

                Assert.AreEqual(-400, Personal.GetBalanceFromToday(Account));
            }

            [TestMethod]
            public void Should_Increase_Balance_When_Transferred_From()
            {
                Account secondAccount = new Account("Test2");

                Personal.AddTransaction(new Transfer(400, Account, secondAccount), new DateTime(2000, 1, 1));

                Assert.AreEqual(400, Personal.GetBalanceFromToday(Account));
                Assert.AreEqual(400, Personal.GetBalanceFromToday(secondAccount));
            }

            [TestMethod]
            public void Should_Decrease_Balance_When_Transferred_To()
            {
                Account secondAccount = new Account("Test2");

                Personal.AddTransaction(new Transfer(400, secondAccount, Account), new DateTime(2000, 1, 1));

                Assert.AreEqual(-400, Personal.GetBalanceFromToday(Account));
                Assert.AreEqual(-400, Personal.GetBalanceFromToday(secondAccount));
            }
        }

        [TestClass]
        public class GetBalanceFromDate : AccountManagerTests
        {
            public Calendar Calendar { get; set; }
            public Account Account { get; set; }

            [TestInitialize]
            public void Init()
            {
                Personal.Accounts.Add(new Account("Test"));

                Calendar = Personal.Calendar;
                Account = Personal.Accounts[0];
            }

            [TestMethod]
            public void Should_Not_Add_Transactions_After_Date()
            {
                Personal.AddTransaction(new Income(400, Account), new DateTime(2001, 1, 2));
                Personal.AddTransaction(new Expense(600, Account), new DateTime(2001, 1, 5));

                Assert.AreEqual(400, Personal.GetBalanceFromDate(Account, new DateTime(2001, 1, 3)));
            }

            [TestMethod]
            public void Should_Not_Apply_Statements_After_Date()
            {
                Personal.AddStatement(new Statement(1000, Account), new DateTime(2001, 1, 1));
                Personal.AddStatement(new Statement(2000, Account), new DateTime(2001, 1, 5));

                Assert.AreEqual(1000, Personal.GetBalanceFromDate(Account, new DateTime(2001, 1, 3)));
            }
        }
    }
}