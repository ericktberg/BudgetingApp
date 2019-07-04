using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions.Tests.Unit.CalendarTests
{
    public class JsonHelper
    {
        public static JsonSerializerSettings Settings { get; } = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
        };


        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        public T DeserializeObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString, Settings);
        }
    }

    [TestClass]
    public class SerializeObject
    {
        JsonHelper Helper { get; } = new JsonHelper();

        [TestMethod]
        public void Should_SerializeEmpty()
        {
            Calendar calendar = new Calendar();

            string s = Helper.SerializeObject(calendar);

            Calendar deserialized = Helper.DeserializeObject<Calendar>(s);

            Assert.AreEqual(0, deserialized.Days.Count());
        }
    }
}
