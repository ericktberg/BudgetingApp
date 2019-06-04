using BudgetingApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Accounts;

namespace BudgetingAppTests
{
    [TestClass]
    public class AddStatementsViewModelTests
    {
        public AddStatementsViewModel ViewModel { get; } = new AddStatementsViewModel(null, new List<Account> { new Account("Account") });

        [TestClass]
        public class Create : AddStatementsViewModelTests
        {
            [TestMethod]
            public void Should_Throw_Exception_When_Date_Unitialized()
            {
                ViewModel.Date = new DateTime();
                
                Assert.ThrowsException<ArgumentException>(ViewModel.Create);
            }

            [TestMethod]
            public void Should_Throw_Exception_When_No_Account_Is_Selected()
            {
                ViewModel.AddToAccount = null;

                Assert.ThrowsException<ArgumentException>(ViewModel.Create);
            }
        }
    }
}
