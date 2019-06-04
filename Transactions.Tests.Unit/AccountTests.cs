using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Transactions.Tests.Unit
{
    [TestClass]
    public class AccountTests
    {
        [TestClass]
        public class GetBalanceFromToday : AccountTests
        {
            public Calendar Calendar => Account.Calendar;
            public Account Account { get; set; }

            [TestInitialize]
            public void Init()
            {
                Account = new Account("Test", AccountType.Liquid);
            }

            [TestMethod]
            public void Should_Have_Zero_Default_Balance()
            {
                Assert.AreEqual(0, Account.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Take_Only_Statement()
            {
                decimal statementBalance = 1000;

                Account.AddStatement(new Statement(statementBalance), new DateTime());

                Assert.AreEqual(statementBalance, Account.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Take_Latest_Statement_Regardless_Of_Order_Added()
            {
                Account.AddStatement(new Statement(1000), new DateTime(1999, 1, 1));
                Account.AddStatement(new Statement(500), new DateTime(1995, 1, 1));

                Assert.AreEqual(1000, Account.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Add_Income_To_Balance()
            {
                Account.Deposit(new Income(400), new DateTime(2000, 1, 1));

                Assert.AreEqual(400, Account.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Subtract_Expenses_From_Balance()
            {
                Account.Withdraw(new Expense(400), new DateTime(2000, 1, 1));

                Assert.AreEqual(-400, Account.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Override_Old_Transactions_With_New_Statements()
            {
                Account.Deposit(new Income(400), new DateTime(2000, 1, 1));
                Account.AddStatement(new Statement(1000), new DateTime(2001, 1, 1));

                Assert.AreEqual(1000, Account.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Add_New_Transactions_To_Old_Statements()
            {
                Account.AddStatement(new Statement(1000), new DateTime(2001, 1, 1));
                Account.Deposit(new Income(400), new DateTime(2001, 1, 2));
                Account.Withdraw(new Expense(600), new DateTime(2001, 1, 3));

                Assert.AreEqual(1000 + 400 - 600, Account.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Maintain_Wealth_With_Transfers()
            {
                var transferAccount = new Account("Test2", AccountType.Liquid);

                Account.TransferTo(new TransferTo(400, transferAccount), new DateTime(2000, 1, 1));

                Assert.AreEqual(400, Account.GetBalanceFromToday());
                Assert.AreEqual(-400, transferAccount.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Add_Transactions_To_Statement_When_BeginningOfDay()
            {
                Account.AddStatement(new Statement(1000) { AddWhen = AddWhen.BeginningOfDay }, new DateTime(2000, 1, 1));
                Account.Deposit(new Income(400), new DateTime(2000, 1, 1));

                Assert.AreEqual(1400, Account.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Overwrite_Transactions_With_Statement_When_EndOfDay()
            {
                Account.AddStatement(new Statement(1000) { AddWhen = AddWhen.EndOfDay }, new DateTime(2000, 1, 1));
                Account.Deposit(new Income(400), new DateTime(2000, 1, 1));

                Assert.AreEqual(1000, Account.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Use_Future_Statements_When_No_Previous_Exist()
            {
                Account.AddStatement(new Statement(1000), new DateTime(2000, 1, 1));
                Account.Withdraw(new Expense(400), new DateTime(1999, 6, 1));

                Assert.AreEqual(1400, Account.GetBalanceFromDate(new DateTime(1999, 1, 1)));
            }

            [TestMethod]
            public void Should_Overwrite_Transactions_With_Statement_When_Beginning_Of_Day_And_Extrapolating_Backwards()
            {
                Account.AddStatement(new Statement(1000) { AddWhen = AddWhen.BeginningOfDay }, new DateTime(2000, 1, 1));
                Account.Withdraw(new Expense(400), new DateTime(2000, 1, 1));

                Assert.AreEqual(1000, Account.GetBalanceFromDate(new DateTime(1999, 1, 1)));
            }
        }

        [TestClass]
        public class DebtInteraction : AccountManagerTests
        {
            public Calendar Calendar => Account.Calendar;
            public Account Account { get; set; }

            [TestInitialize]
            public void Init()
            {
                Account = new DebtAccount("Test");
            }

            [TestMethod]
            public void Should_Decrease_Debt_Balance_From_Income()
            {
                Account.Deposit(new Income(400), new DateTime(2000, 1, 1));

                Assert.AreEqual(-400, Account.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Increase_Balance_When_Transferred_From()
            {
                Account secondAccount = new Account("Test2", AccountType.Liquid);

                Account.TransferFrom(new TransferFrom(400, secondAccount), new DateTime(2000, 1, 1));

                Assert.AreEqual(400, Account.GetBalanceFromToday());
                Assert.AreEqual(400, secondAccount.GetBalanceFromToday());
            }

            [TestMethod]
            public void Should_Decrease_Balance_When_Transferred_To()
            {
                Account secondAccount = new Account("Test2", AccountType.Liquid);

                Account.TransferTo(new TransferTo(400, secondAccount), new DateTime(2000, 1, 1));

                Assert.AreEqual(-400, Account.GetBalanceFromToday());
                Assert.AreEqual(-400, secondAccount.GetBalanceFromToday());
            }
        }

        [TestClass]
        public class GetBalanceFromDate : AccountManagerTests
        {
            public Calendar Calendar => Account.Calendar;
            public Account Account { get; set; }

            [TestInitialize]
            public void Init()
            {
                Account = new Account("Test", AccountType.Liquid);
            }

            [TestMethod]
            public void Should_Not_Add_Transactions_After_Date()
            {
                Account.Deposit(new Income(400), new DateTime(2001, 1, 2));
                Account.Withdraw(new Expense(600), new DateTime(2001, 1, 5));

                Assert.AreEqual(400, Account.GetBalanceFromDate(new DateTime(2001, 1, 3)));
            }

            [TestMethod]
            public void Should_Not_Apply_Statements_After_Date()
            {
                Account.AddStatement(new Statement(1000), new DateTime(2001, 1, 1));
                Account.AddStatement(new Statement(2000), new DateTime(2001, 1, 5));

                Assert.AreEqual(1000, Account.GetBalanceFromDate(new DateTime(2001, 1, 3)));
            }
        }
    }
}
