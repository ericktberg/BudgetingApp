using Newtonsoft.Json;

namespace Sunsets.Transactions
{
    public class Statement
    {
        public Statement(decimal balance, AddWhen addWhen = AddWhen.StartOfDay)
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