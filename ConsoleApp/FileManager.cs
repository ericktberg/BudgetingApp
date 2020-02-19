using Sunsets.Transactions;
using System.IO;

namespace Sunsets.ConsoleApp
{
    /// <summary>
    /// Manage files so we don't have to interact with streams or file IO anywhere else
    /// </summary>
    public class FileManager
    {

        private bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        private FileStream ReadFileStream(string filePath)
        {
            if (!FileExists(filePath))
            {
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                return File.Create(filePath);
            }
            else
            {
                return File.OpenRead(filePath);
            }
        }

        private FileStream WriteFileStream(string filePath)
        {
            if (!FileExists(filePath))
            {
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                return File.Create(filePath);
            }
            else
            {
                return File.OpenWrite(filePath);
            }
        }

        public AccountManager GetAccountManagerFromPath(string filePath)
        {
            AccountManager account;

            using (Stream fileStream = ReadFileStream(filePath))
            {
                account = AccountManager.FromJson(fileStream);
            }

            return account ?? new AccountManager();
        }

        public bool SaveAccountManagerToPath(string filePath, AccountManager account)
        {
            using (Stream fileStream = WriteFileStream(filePath))
            {
                return account.ToJson(fileStream);
            }
        }
    }
}