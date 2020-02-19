using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

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
            BackingAccounts = accounts;
        }

        public IList<Account> BackingAccounts { get; }

        [JsonIgnore]
        public IEnumerable<Account> Accounts => BackingAccounts;

        public Account GetAccount(string accountName)
        {
            return Accounts.First(a => a.Name == accountName);
        }

        public bool AddAccount(Account account)
        {
            if (BackingAccounts.Any(a => a.Name == account.Name)) throw new InvalidOperationException("Same account name");

            BackingAccounts.Add(account);
            return true;
        }

        public bool RemoveAccount(Account account)
        {
            return BackingAccounts.Remove(account);
        }

        public decimal GetBalanceFromDate(Account account, DateTime date)
        {
            return account.GetBalanceFromDate(date);
        }

        public bool Transfer(Account transferFrom, Account transferTo, decimal amount, DateTime date)
        {
            transferFrom.AddTransaction(new TransferFrom(amount, transferTo), date);
            transferTo.AddTransaction(new TransferTo(amount, transferFrom), date);

            return true;
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