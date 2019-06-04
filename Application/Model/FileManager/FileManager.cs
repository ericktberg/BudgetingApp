using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunsets.Transactions;

namespace Sunsets.Application.Model.FileManager
{
    /// <summary>
    /// Manage files so we don't have to interact with streams or file IO anywhere else
    /// </summary>
    public class FileManager : IManageFiles
    {
        public string DefaultFileName => "Finance.json";

        public string DefaultFileLocation => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public string DefaultFilePath => Path.Combine(DefaultFileLocation, DefaultFileName);

        private bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        private FileStream ReadFileStream(string filePath)
        {
            if (!FileExists(filePath))
            {
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
