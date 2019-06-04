using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions.Accounts;

namespace Sunsets.Transactions.Tests.Unit.AccountTests
{
    [TestClass]
    public class AddRecurringTransaction
    {
        public Calendar Calendar => Account.Calendar;
        public Account Account { get; set; }
    }
}
