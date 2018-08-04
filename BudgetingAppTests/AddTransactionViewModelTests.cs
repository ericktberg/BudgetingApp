using BudgetingApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions;
using Transactions.Accounts;

namespace BudgetingAppTests
{
    [TestClass]
    public class AddTransactionViewModelTests
    {
        public AddTransactionViewModel ViewModel { get; set; } = new AddExpenseViewModel(null, new List<Account> { new Account("Test") });

        [TestClass]
        public class Constructor : AddTransactionViewModelTests
        {
            [TestMethod]
            public void Should_Copy_Values_Between_Types()
            {
                ViewModel.Amount = 400;
                ViewModel.Date = DateTime.Now;

                var copied = new AddTransferViewModel(ViewModel);

                Assert.AreEqual(ViewModel.Amount, copied.Amount);
                Assert.AreEqual(ViewModel.Date, copied.Date);
                Assert.AreEqual(ViewModel.AddTo, copied.AddTo);
                Assert.AreEqual(ViewModel.Accounts, copied.Accounts);

            }
        }

        [TestClass]
        public class Create : AddTransactionViewModelTests
        {
            [TestMethod]
            public void Should_Throw_Exception_When_Date_Unitialized()
            {
                ViewModel.Date = new DateTime();

                Assert.ThrowsException<ArgumentException>(ViewModel.Create);
            }

            [TestMethod]
            public void Should_Be_Income_When_Type_Selected_Is_Income()
            {
                ViewModel = new AddIncomeViewModel(null, new List<Account> { new Account("Test") })
                {
                    Date = DateTime.Now,
                    DepositAccount = new Account("Test")
                };

                Assert.IsInstanceOfType(ViewModel.Create(), typeof(Income));
            }

            [TestMethod]
            public void Should_Be_Expense_When_Type_Selected_Is_Expense()
            {
                ViewModel = new AddExpenseViewModel(null, new List<Account> { new Account("Test") })
                {
                    Date = DateTime.Now,
                    WithdrawAccount = new Account("Test"),
                };

                Assert.IsInstanceOfType(ViewModel.Create(), typeof(Expense));
            }

            [TestMethod]
            public void Should_Be_Transfer_When_Type_Is_Transfer()
            {
                ViewModel = new AddTransferViewModel(null, new List<Account> { new Account("Test"), new Account("Second") })
                {
                    Date = DateTime.Now,
                    WithdrawAccount = new Account("Test"),
                    DepositAccount = new Account("Second"),
                };

                Assert.IsInstanceOfType(ViewModel.Create(), typeof(TransferTo));
            }
        }
    }
}
