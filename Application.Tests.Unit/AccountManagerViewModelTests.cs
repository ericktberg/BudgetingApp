using BudgetingApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BudgetingAppTests
{
    [TestClass]
    public class AccountManagerViewModelTests
    {
        public static MockFileManager Files { get; } = new MockFileManager();

        public AccountManagerViewModel ViewModel { get; } = new AccountManagerViewModel(Files);

        [TestClass]
        public class Constructor : AccountManagerViewModelTests
        {
            [TestMethod]
            public void Should_Never_Have_Null_Manager_On_Load()
            {
                Assert.IsNotNull(ViewModel.AccountManager);
            }

            [TestMethod]
            public void Should_Save_Manager_On_Shutdown()
            {
                ViewModel.OnShutdown();

                Assert.IsTrue(Files.IsSaved);
                // FileManager.Verify(_ => _.SaveAccountManagerToPath(It.IsAny<string>(), It.IsNotNull<AccountManager>()), Times.Once);
            }
        }
    }
}
