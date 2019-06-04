using Sunsets.Application.Model.FileManager;
using Sunsets.Transactions;

namespace Sunsets.Application.Tests.Unit
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
