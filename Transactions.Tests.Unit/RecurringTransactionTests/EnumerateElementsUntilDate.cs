using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Sunsets.Transactions.Tests.Unit.RecurringTransactionTests
{
    [TestClass]
    public class EnumerateElementsUntilDate
    {
        RecurringTransactionTester Tester { get; } = new RecurringTransactionTester();

        [TestMethod]
        public void Should_EnumerateNothing_If_UntilDate_Before_StartDate()
        {
            RecurringTransaction t = new RecurringTransaction(new Income(500), Tester.MockFrequency.Object, new DateTime(2019, 1, 1), null);

            Tester.MockFrequency.DateCollection.Add(new DateTime(2019, 1, 2));

            t.EnumerateElementsUntilDate(new DateTime(2018, 1, 1));

            Assert.AreEqual(0, t.Elements.Count());
        }

        [TestMethod]
        public void Should_EnumerateNothing_If_FrequencyListsNoDates()
        {
            RecurringTransaction t = new RecurringTransaction(new Income(500), Tester.MockFrequency.Object, new DateTime(2019, 1, 1), null);

            t.EnumerateElementsUntilDate(new DateTime(2018, 1, 1));

            Assert.AreEqual(0, t.Elements.Count());
        }

        [TestMethod]
        public void Should_EnumerateNothing_If_FrequencyListsDates_Before_StartDate()
        {
            RecurringTransaction t = new RecurringTransaction(new Income(500), Tester.MockFrequency.Object, new DateTime(2019, 1, 1), null);

            Tester.MockFrequency.DateCollection.Add(new DateTime(2018, 1, 1));

            t.EnumerateElementsUntilDate(new DateTime(2019, 3, 1));

            Assert.AreEqual(0, t.Elements.Count());
        }

        [TestMethod]
        public void Should_EnumerateNothing_If_FrequencyListsDates_After_EndDate()
        {
            RecurringTransaction t = new RecurringTransaction(new Income(500), 
                Tester.MockFrequency.Object, 
                new DateTime(2019, 1, 1), 
                new DateTime(2019, 2, 1));

            Tester.MockFrequency.DateCollection.Add(new DateTime(2019, 2, 15));

            t.EnumerateElementsUntilDate(new DateTime(2019, 3, 1));

            Assert.AreEqual(0, t.Elements.Count());
        }

        [TestMethod]
        public void Should_EnumerateElement_On_StartDate()
        {
            RecurringTransaction t = new RecurringTransaction(new Income(500), Tester.MockFrequency.Object, new DateTime(2019, 1, 1), null);

            Tester.MockFrequency.DateCollection.Add(new DateTime(2019, 1, 1));

            t.EnumerateElementsUntilDate(new DateTime(2019, 3, 1));

            Assert.AreEqual(1, t.Elements.Count());
        }

        [TestMethod]
        public void ShouldNot_EnumerateElement_Twice()
        {
            RecurringTransaction t = new RecurringTransaction(new Income(500), Tester.MockFrequency.Object, new DateTime(2019, 1, 1), null);

            Tester.MockFrequency.DateCollection.Add(new DateTime(2019, 1, 1));

            t.EnumerateElementsUntilDate(new DateTime(2019, 3, 1));
            t.EnumerateElementsUntilDate(new DateTime(2019, 3, 1));

            Assert.AreEqual(1, t.Elements.Count());
        }

        [TestMethod]
        public void Should_EnumerateConsecutiveElements()
        {
            RecurringTransaction t = new RecurringTransaction(new Income(500), Tester.MockFrequency.Object, new DateTime(2019, 1, 1), null);

            Tester.MockFrequency.DateCollection.Add(new DateTime(2019, 1, 1));
            Tester.MockFrequency.DateCollection.Add(new DateTime(2019, 1, 5));
            Tester.MockFrequency.DateCollection.Add(new DateTime(2019, 1, 10));

            t.EnumerateElementsUntilDate(new DateTime(2019, 3, 1));

            Assert.AreEqual(3, t.Elements.Count());
        }
    }
}