using Newtonsoft.Json;

namespace Sunsets.Transactions.Accounts
{
    public class Statement
    {
        public Statement(decimal balance, AddWhen addWhen = AddWhen.BeginningOfDay)
        {
            Balance = balance;
            AddWhen = addWhen;
        }

        [JsonProperty]
        public AddWhen AddWhen { get; set; }

        [JsonProperty]
        public decimal Balance { get; set; }
    }
}