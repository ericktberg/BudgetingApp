namespace Sunsets.Transactions
{
    public interface IHaveBalance
    {
        decimal EndingBalance { get; }

        bool HasStatement { get; }

        decimal StartingBalance { get; }

        bool StatementOnLeft();

        bool StatementOnRight();

        void UpdateBalance();
    }
}