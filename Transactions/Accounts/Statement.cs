using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Accounts
{
    public class Statement
    {
        [JsonProperty]
        public decimal Balance { get; set; }


        [JsonProperty]
        public Account Account { get; set; }

        public Statement(decimal balance, Account account)
        {
            Balance = balance;
            Account = account;
        }
    }
}
