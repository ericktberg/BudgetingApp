using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sunsets.Transactions.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions.Tests.Unit.Import.CsvFileTests
{
    public class ImportTester
    {

    }

    [TestClass]
    public class Load
    {

        public string CsvString { get; set; } =
            "field1,field2\n"
            + "a,1\n"
            + "b,2\n";

        public MemoryStream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();

            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        [TestMethod]
        public void Should_LoadRows()
        {
            var c = CsvFile.Load(GenerateStreamFromString(CsvString));

            Assert.AreEqual(2, c.Rows.Count);
            Assert.AreEqual("a", c.Rows[0].Columns[0]);
            Assert.AreEqual("1", c.Rows[0].Columns[1]);

            Assert.AreEqual("b", c.Rows[1].Columns[0]);
            Assert.AreEqual("2", c.Rows[1].Columns[1]);
        }

        [TestMethod]
        public void Should_ReadHeader()
        {
            var c = CsvFile.Load(GenerateStreamFromString(CsvString));

            Assert.AreEqual("field1", c.Header[0]);
            Assert.AreEqual("field2", c.Header[1]);
        }
    }
}
