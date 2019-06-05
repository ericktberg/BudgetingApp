using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions.Accounts;
using System;
using Moq;

namespace Sunsets.Transactions.Tests.Unit.AccountTests
{
    public class AccountTester
    {
        public AccountTester()
        {
            Account = new Account("TestAccount", AccountType.Liquid);
            MockIncomeFrequency = new Mock<IFrequency>();
            MockExpenseFrequency = new Mock<IFrequency>();
        }

        public Mock<IFrequency> MockIncomeFrequency { get; }

        public Mock<IFrequency> MockExpenseFrequency { get; }

        public Account Account { get; }

        public Account GetAccount()
        {
            return Account;
        }

        public void SetElapsedTimes(Mock<IFrequency> mock, int elapsedTimes)
        {
            mock.Setup(_ => _.ElapsedEvents(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(elapsedTimes);
        }
        
        public RecurringTransaction IncomeTransaction()
        {
            return new RecurringTransaction(new Income(500), MockIncomeFrequency.Object, new DateTime(2019, 1, 7), null);
        }

        public RecurringTransaction ExpenseTransaction()
        {
            return new RecurringTransaction(new Expense(300), MockExpenseFrequency.Object, new DateTime(2019, 1, 10), null);
        }

        public Calendar Calendar => Account.Calendar;
    }

    [TestClass]
    public class AddRecurringTransaction
    {
        public AccountTester Tester { get; } = new AccountTester();

        [TestMethod]
        public void Should_ComputeValue_From_ElapsedTimes()
        {
            Account account = Tester.GetAccount();

            var transaction = Tester.IncomeTransaction();
            account.AddRecurringTransaction(transaction);

            Tester.SetElapsedTimes(Tester.MockIncomeFrequency, 0);
            Assert.AreEqual(0, account.GetBalanceFromDate(transaction.StartDate));

            Tester.SetElapsedTimes(Tester.MockIncomeFrequency, 1);
            Assert.AreEqual(500, account.GetBalanceFromDate(transaction.StartDate));

            Tester.SetElapsedTimes(Tester.MockIncomeFrequency, 2);
            Assert.AreEqual(1000, account.GetBalanceFromDate(transaction.StartDate));

            Tester.SetElapsedTimes(Tester.MockIncomeFrequency, 5);
            Assert.AreEqual(2500, account.GetBalanceFromDate(transaction.StartDate));
        }
        
        [TestMethod]
        public void Should_ComputeValue_From_MultipleRecurring_Transactions()
        {
            Account account = Tester.GetAccount();

            var income = Tester.IncomeTransaction();
            account.AddRecurringTransaction(income);

            var expense = Tester.ExpenseTransaction();
            account.AddRecurringTransaction(expense);

            Tester.SetElapsedTimes(Tester.MockIncomeFrequency, 1);
            Tester.SetElapsedTimes(Tester.MockExpenseFrequency, 1);
            Assert.AreEqual(200, account.GetBalanceFromDate(expense.StartDate));

            Tester.SetElapsedTimes(Tester.MockIncomeFrequency, 2);
            Assert.AreEqual(700, account.GetBalanceFromDate(income.StartDate.AddDays(7)));

            Tester.SetElapsedTimes(Tester.MockExpenseFrequency, 2);
            Assert.AreEqual(400, account.GetBalanceFromDate(expense.StartDate.AddDays(7)));

            Tester.SetElapsedTimes(Tester.MockIncomeFrequency, 5);
            Assert.AreEqual(1900, account.GetBalanceFromDate(expense.StartDate.AddDays(7)));
        }

    }
}
