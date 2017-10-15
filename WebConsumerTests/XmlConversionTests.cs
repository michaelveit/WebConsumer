using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WebConsumer;

namespace WebConsumer.UnitTests
{
    [TestFixture]
    public class XmlConversionTests
    {
        private static readonly string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <query xmlns:yahoo=\"http://www.yahooapis.com/v1/base.rng\" yahoo:count=\"1\" yahoo:created=\"2017-10-14T14:38:31Z\" yahoo:lang=\"en-US\"><results><channel><yweather:wind xmlns:yweather=\"http://xml.weather.yahoo.com/ns/rss/1.0\" chill=\"59\" direction=\"150\" speed=\"18\"/></channel></results></query><!-- total: 10 -->";

        [Test]
        public void Can_Convert_Xml_And_Parse_Json()
        {
            var json = XmlToJson.Convert(xml);
            var selector = "query.results.channel.wind.chill,query.results.channel.wind.speed";
            var parser = new JsonObjectParser();
            var jsonString = parser.Parse(json, selector);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);

            Assert.IsNotNull(result, "result was null");
            Assert.AreEqual("59", result.chill, "chill was not correct.");
            Assert.AreEqual("18", result.speed, "speed was not correct.");

        }

    }
}
