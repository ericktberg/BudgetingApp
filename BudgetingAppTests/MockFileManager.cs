using BudgetingApp.Model.FileManager;
using Transactions;

namespace BudgetingAppTests
{
    public class MockFileManager : IManageFiles
    {
        public string DefaultFileName => "Default";

        public string DefaultFileLocation => "Default";

        public string DefaultFilePath => "Default";

        public AccountManager GetAccountManagerFromPath(string filePath)
        {
            return new AccountManager();
        }

        public bool IsSaved { get; set; }

        public bool SaveAccountManagerToPath(string filePath, AccountManager account)
        {
            IsSaved = true;
            return true;
        }
    }
}
