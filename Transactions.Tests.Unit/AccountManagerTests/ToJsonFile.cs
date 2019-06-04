using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions.Accounts;
using System;
using System.IO;
using System.Linq;

namespace Sunsets.Transactions.Tests.Unit.AccountManagerTests
{
    [TestClass]
    public class ToJsonFile
    {
        public static string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static string path = Path.Combine(folder, "test.json");

        public AccountManager Personal => Tester.Personal;

        public AccountManagerTester Tester { get; } = new AccountManagerTester();

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        [TestInitialize]
        public void Init()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        [TestMethod]
        public void Should_Create_Readable_Stream()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Personal.ToJson(stream);

                stream.Position = 0;
                AccountManager duplicate = AccountManager.FromJson(stream);

                Assert.IsNotNull(duplicate);
            }
        }

        [TestMethod]
        public void Should_Read_From_File()
        {
            using (FileStream stream = File.Create(path))
            {
                Personal.ToJson(stream);

                Assert.IsTrue(File.Exists(path));
            }

            using (FileStream stream = File.OpenRead(path))
            {
                AccountManager duplicate = AccountManager.FromJson(stream);

                Assert.IsNotNull(duplicate);
            }
        }

        [TestMethod]
        public void Should_Save_And_Load_Days_Date()
        {
            Account mainAccount = new Account("TestAccount", AccountType.Liquid);
            Personal.Accounts.Add(mainAccount);
            mainAccount.Calendar.GetDayForDate(new DateTime(2001, 2, 3));

            using (MemoryStream stream = new MemoryStream())
            {
                Personal.ToJson(stream);

                stream.Position = 0;
                AccountManager duplicate = AccountManager.FromJson(stream);

                Assert.AreEqual(1, duplicate.Accounts.First().Calendar.Days.Count());

                Assert.AreEqual(Personal.Accounts.First().Calendar.Days.First().Date, duplicate.Accounts.First().Calendar.Days.First().Date);
            }
        }
    }
}