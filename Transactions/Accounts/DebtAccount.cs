namespace Sunsets.Transactions.Accounts
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