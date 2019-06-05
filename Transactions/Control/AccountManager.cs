using Newtonsoft.Json;
using Sunsets.Transactions.Accounts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sunsets.Transactions
{
    /// <summary>
    /// <see cref="AccountManager"/> is the sum-total list of all accounts.
    /// This could be seen akin to a users list of accounts, but it's all for me right now.
    /// </summary>
    public class AccountManager
    {
        public AccountManager() : this(new List<Account>())
        {
        }

        private AccountManager(IList<Account> accounts)
        {
            Accounts = accounts;
        }

        public IList<Account> Accounts { get; private set; }

        public decimal GetBalanceFromDate(Account account, DateTime date)
        {
            return account.GetBalanceFromDate(date);
        }
        
        #region IO Methods

        public static JsonSerializerSettings Settings { get; } = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
        };

        public static AccountManager FromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<AccountManager>(jsonString, Settings);
        }

        public static AccountManager FromJson(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            JsonSerializer serializer = JsonSerializer.Create(Settings);
            return (AccountManager) serializer.Deserialize(reader, typeof(AccountManager));
        }

        public bool ToJson(Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream, new UnicodeEncoding());
            JsonSerializer serializer = JsonSerializer.Create(Settings);
            serializer.Serialize(writer, this);
            writer.Flush();

            return true;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Settings);
        }

        #endregion IO Methods
    }
}