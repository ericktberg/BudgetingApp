using System;
using Sunsets.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions.Import;
using System.IO;

namespace Sunsets.Transactions.Tests.Integration
{
    [TestClass]
    public class ChaseTestImport
    {
        [TestMethod]
        public void TestImport()
        {
            Account account = new LiabilityAccount("Amazon Signature Credit");

            CsvImporter importer = new CsvImporter();

            CsvFile file = CsvFile.Load(File.OpenRead("Chase_TestData.CSV"));

            importer.Import(file, account);

            account.AddStatement(new Statement(new decimal(-3638.41), AddWhen.StartOfDay), DateTime.Today);

            AccountManager manager = new AccountManager();
            manager.AddAccount(account);

            using (var s = File.OpenWrite(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TestJson.json")))
            {
                manager.ToJson(s);
            }
        }
    }
}
