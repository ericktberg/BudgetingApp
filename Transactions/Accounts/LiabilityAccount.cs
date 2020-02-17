namespace Sunsets.Transactions
{
    public class LiabilityAccount : Account
    {
        public LiabilityAccount(string name) : base(name, AccountType.Liability)
        {
        }

        public override decimal GetDelta(decimal amount)
        {
            return -base.GetDelta(amount);
        }
    }
}