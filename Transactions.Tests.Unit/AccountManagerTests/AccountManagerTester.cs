namespace Sunsets.Transactions.Tests.Unit.AccountManagerTests
{

    public class AccountManagerTester
    {
        public AccountManagerTester()
        {
            Savings = new Account("Savings", AccountType.Liquid);
            Checking = new Account("Checking", AccountType.Liquid);
            Credit = new DebtAccount("Credit");

            Manager.Accounts.Add(Savings);
            Manager.Accounts.Add(Checking);
            Manager.Accounts.Add(Credit);
        }

        public AccountManager Manager { get; } = new AccountManager();


        public Account Savings { get; }

        public Account Checking { get; }

        public Account Credit { get; }
    }
}
