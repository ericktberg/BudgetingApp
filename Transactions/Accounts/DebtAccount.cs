namespace Sunsets.Transactions
{
    public class DebtAccount : Account
    {
        public DebtAccount(string name) : base(name, AccountType.Debt)
        {
        }

        public override decimal GetDelta(decimal amount)
        {
            return -base.GetDelta(amount);
        }
    }
}