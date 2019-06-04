using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunsets.Transactions;

namespace Sunsets.Application.Model.FileManager
{
    public interface IManageFiles
    {
        string DefaultFileName { get; }

        string DefaultFileLocation { get; }

        string DefaultFilePath { get; }

        AccountManager GetAccountManagerFromPath(string filePath);

        bool SaveAccountManagerToPath(string filePath, AccountManager account);
    }
}
