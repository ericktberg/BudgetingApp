using BudgetingApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class AddAccountsViewModelTests
    {
        public static AccountManager Manager { get; } = new AccountManager()
        {
            Accounts = { new Account("Taken") }
        };

        public static Mock<IAddAccounts> AddAccounts = new Mock<IAddAccounts>();

        public AddAccountsViewModel ViewModel { get; set; } = new AddAccountsViewModel(AddAccounts.Object, new List<Account> { new Account("Taken") });

        [TestClass]
        public class NameTaken : AddAccountsViewModelTests
        {
            
            [TestMethod]
            public void Should_Be_True_If_AccountName_Already_Exists()
            {
                ViewModel.AccountName = "Taken";
                Assert.IsTrue(ViewModel.NameTaken);
            }

            [TestMethod]
            public void Should_Be_False_If_AccountName_Is_Free()
            {
                ViewModel.AccountName = "FreeName";
                Assert.IsFalse(ViewModel.NameTaken);
            }
        }

        [TestClass]
        public class Create : AddAccountsViewModelTests
        {
            [TestMethod]
            public void Should_Throw_Exception_If_Name_Is_Taken()
            {
                ViewModel.AccountName = "Taken";

                Assert.ThrowsException<ArgumentException>(ViewModel.Create);
            }

            [TestMethod]
            public void Should_Return_A_New_Account_With_Name_Of_AccountName()
            {
                ViewModel.AccountName = "Test";

                Assert.AreEqual("Test", ViewModel.Create().Name);
            }

            [TestMethod]
            public void Should_Return_Debt_Account_If_Type_Is_Debt()
            {
                ViewModel.AccountName = "Test";
                ViewModel.Type = AccountType.Debt;

                Assert.IsInstanceOfType(ViewModel.Create(), typeof(DebtAccount));
            }
        } 
    }
}
