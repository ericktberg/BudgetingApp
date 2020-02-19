namespace Sunsets.Transactions.Tests.Unit.AccountManagerTests
{

    public class AccountManagerTester
    {
        public AccountManagerTester()
        {
            Savings = new Account("Savings", AccountType.Asset);
            Checking = new Account("Checking", AccountType.Asset);
            Credit = new LiabilityAccount("Credit");

            Manager.AddAccount(Savings);
            Manager.AddAccount(Checking);
            Manager.AddAccount(Credit);
        }

        public AccountManager Manager { get; } = new AccountManager();


        public Account Savings { get; }

        public Account Checking { get; }

        public Account Credit { get; }
    }
}
