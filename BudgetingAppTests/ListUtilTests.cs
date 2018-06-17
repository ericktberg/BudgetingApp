using System;
using System.Collections.Generic;
using BudgetingApp.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BudgetingAppTests
{
    [TestClass]
    public class ListUtilTests
    {
        [TestClass]
        public class MatchListChanges
        {
            public IList<string> ToChange { get; } = new List<string>();

            public IList<int> ToMatch { get; } = new List<int>();

            [TestMethod]
            public void Should_Match_Add()
            {
                ToMatch.Add(1);

                Assert.AreEqual(0, ToChange.Count);
                Assert.IsFalse(ToChange.Contains("1"));

                ListUtils.MatchListChanges(ToChange, ToMatch, n => n.ToString(), s => Int32.Parse(s));

                Assert.AreEqual(1, ToChange.Count);
                Assert.IsTrue(ToChange.Contains("1"));
            }

            [TestMethod]
            public void Should_Match_Remove()
            {
                ToChange.Add("1");

                Assert.AreEqual(1, ToChange.Count);
                Assert.IsTrue(ToChange.Contains("1"));

                ListUtils.MatchListChanges(ToChange, ToMatch, n => n.ToString(), s => Int32.Parse(s));

                Assert.AreEqual(0, ToChange.Count);
                Assert.IsFalse(ToChange.Contains("1"));
            }

            [TestMethod]
            public void Should_Match_Insert()
            {
                ToChange.Add("1");
                ToMatch.Add(1);

                ToMatch.Insert(0, 2);

                Assert.AreEqual(1, ToChange.Count);
                Assert.IsFalse(ToChange.Contains("2"));

                ListUtils.MatchListChanges(ToChange, ToMatch, n => n.ToString(), s => Int32.Parse(s));

                Assert.AreEqual(2, ToChange.Count);
                Assert.IsTrue(ToChange.Contains("2"));
                Assert.AreEqual(0, ToChange.IndexOf("2"));
            }

            [TestMethod]
            public void Should_Match_Insert_From_Long_List()
            {
                ToChange.Add("1");

                ToMatch.Add(2);
                ToMatch.Add(1);
                ToMatch.Add(3);

                Assert.AreEqual(0, ToChange.IndexOf("1"));

                ListUtils.MatchListChanges(ToChange, ToMatch, n => n.ToString(), s => Int32.Parse(s));

                Assert.AreEqual(0, ToChange.IndexOf("2"));
                Assert.AreEqual(1, ToChange.IndexOf("1"));
                Assert.AreEqual(2, ToChange.IndexOf("3"));
            }

            [TestMethod]
            public void Should_Match_Total_Change()
            {
                ToChange.Add("1");
                ToChange.Add("4");

                ToMatch.Add(2);
                ToMatch.Add(3);

                Assert.AreEqual(0, ToChange.IndexOf("1"));
                Assert.AreEqual(1, ToChange.IndexOf("4"));

                ListUtils.MatchListChanges(ToChange, ToMatch, n => n.ToString(), s => Int32.Parse(s));

                Assert.AreEqual(0, ToChange.IndexOf("2"));
                Assert.AreEqual(1, ToChange.IndexOf("3"));
                Assert.AreEqual(-1, ToChange.IndexOf("1"));
                Assert.AreEqual(-1, ToChange.IndexOf("4"));
            }

            [TestMethod]
            public void Should_Match_Move()
            {
                ToChange.Add("1");
                ToChange.Add("2");

                ToMatch.Add(2);
                ToMatch.Add(1);

                Assert.AreEqual(0, ToChange.IndexOf("1"));
                Assert.AreEqual(1, ToChange.IndexOf("2"));

                ListUtils.MatchListChanges(ToChange, ToMatch, n => n.ToString(), s => Int32.Parse(s));

                Assert.AreEqual(0, ToChange.IndexOf("2"));
                Assert.AreEqual(1, ToChange.IndexOf("1"));
            }
        }
    }
}
