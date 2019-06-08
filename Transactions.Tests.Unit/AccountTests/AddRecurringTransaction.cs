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
        }
        
        public Account Account { get; }

        public Account GetAccount()
        {
            return Account;
        }
                
        public RecurringTransaction IncomeTransaction()
        {
            return new RecurringTransaction(new Income(500), new MonthlyEvent(1), new DateTime(2019, 1, 1), null);
        }

        public RecurringTransaction ExpenseTransaction()
        {
            return new RecurringTransaction(new Expense(300), new MonthlyEvent(5), new DateTime(2019, 1, 1), null);
        }

        public Calendar Calendar => Account.Calendar;
    }

    [TestClass]
    public class AddRecurringTransaction
    {
        public AccountTester Tester { get; } = new AccountTester();
        
        
        [TestMethod]
        public void Scenario_ComputeValue_From_MultipleRecurring_Transactions()
        {
            Account account = Tester.GetAccount();

            var income = Tester.IncomeTransaction();
            var expense = Tester.ExpenseTransaction();

            DateTime day1 = new DateTime(2019, 1, 2);
            DateTime day2 = new DateTime(2019, 1, 6);
            DateTime day3 = day1.AddMonths(1);
            DateTime day4 = day2.AddMonths(1);

            income.EnumerateElementsUntilDate(day4);
            expense.EnumerateElementsUntilDate(day4);

            account.AddRecurringTransaction(income);
            account.AddRecurringTransaction(expense);
            

            Assert.AreEqual(500, account.GetBalanceFromDate(day1));
            Assert.AreEqual(200, account.GetBalanceFromDate(day2));
            Assert.AreEqual(700, account.GetBalanceFromDate(day3));
            Assert.AreEqual(400, account.GetBalanceFromDate(day4));
        }

    }
}
