using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Accounts
{
    public enum AddWhen
    {
        BeginningOfDay,
        EndOfDay,
    }

    public class Statement
    {
        [JsonProperty]
        public decimal Balance { get; set; }
       
        [JsonProperty]
        public AddWhen AddWhen { get; set; }

        public Statement(decimal balance, AddWhen addWhen = AddWhen.BeginningOfDay)
        {
            Balance = balance;
            AddWhen = addWhen;
        }
    }
}
