using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebConsumer;

namespace WebConsumer.UnitTests
{
    [TestFixture]
    public class JSONParserTests
    {


    private static readonly string JSON = @"{  
   'query':{  
      'count':1,
      'created':'2017-10-11T16:34:04Z',
      'lang':'en-US',
      'results':{  
         'channel':{  
            'wind':{  
               'chill':'52',
               'direction':'55',
               'speed':'32'
            }
         }
      }
   }
}";

    private static readonly string  JSON_WITH_ARRAYS = @"{
   'channel': {
     'title': 'James Newton-King',
     'link': 'http://james.newtonking.com',
     'description': 'James Newton-King\'s blog.',
     'item': [
       {
         'title': 'Json.NET 1.3 + New license + Now on CodePlex',
         'description': 'Annoucing the release of Json.NET 1.3, the MIT license and the source on CodePlex',
        'link': 'http://james.newtonking.com/projects/json-net.aspx',
        'categories': [
          'Json.NET',
          'CodePlex'
        ]
      },
      {
        'title': 'LINQ to JSON beta',
        'description': 'Annoucing LINQ to JSON',
        'link': 'http://james.newtonking.com/projects/json-net.aspx',
        'categories': [
          'Json.NET',
          'LINQ'
        ]
      }
    ]
  }
}";


        [Test]
        public void Bad_Path_Throws_Exception()
        {
            var selector = "query.results.channel.wind.xxxx,query.results.channel.wind.speed";
            var parser = new JsonObjectParser();
            var ex = Assert.Throws<Exception>(delegate { parser.Parse(JSON, selector); });
            Assert.AreEqual(ex.Message, "The selector path query.results.channel.wind.xxxx was not found.", "Wrong exception was thrown");
        }
        [Test]
        public void Bad_Array_Path_Throws_Exception()
        {
            var selector = "channel.item[*].bad";
            var parser = new JsonObjectParser();
            var ex = Assert.Throws<Exception>(delegate { parser.Parse(JSON_WITH_ARRAYS, selector); });
            Assert.AreEqual(ex.Message, "The selector path channel.item[*].bad was not found.", "Wrong exception was thrown");
        }


        [Test]
        public void Empty_Template_Returns_Full_Json()
        {

            var selector = string.Empty;
            var parser = new JsonObjectParser();
            var jsonString = parser.Parse(JSON, selector);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);

            Assert.IsNotNull(result, "result was null");
            Assert.AreEqual("52", result.query.results.channel.wind.chill, "chill was not correct.");
            Assert.AreEqual("32", result.query.results.channel.wind.speed, "speed was not correct.");
            Assert.AreEqual("55", result.query.results.channel.wind.direction, "direction was not correct.");

        }


        [Test]
        public void Can_Create_From_Json()
        {
  
            var selector = "query.results.channel.wind.chill,query.results.channel.wind.speed";
            var parser = new JsonObjectParser();
            var jsonString = parser.Parse(JSON, selector);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);

            Assert.IsNotNull(result, "result was null");
            Assert.AreEqual("52", result.chill, "chill was not correct.");
            Assert.AreEqual("32", result.speed, "speed was not correct.");
        }

        [Test]
        public void Can_Parse_Json_With_Arrays()
        {
            var selector = "channel.item[*].title";
            var parser = new JsonObjectParser();
            var jsonString = parser.Parse(JSON_WITH_ARRAYS, selector);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);

            Assert.IsNotNull(result, "result was null");
            Assert.IsNotNull(result.title, "title was null");
            Assert.AreEqual(2, result.title.Count, "title length not correct.");
            Assert.AreEqual("Json.NET 1.3 + New license + Now on CodePlex", result.title[0], "first title is not correct.");
            Assert.AreEqual("LINQ to JSON beta", result.title[1], "second title is not correct.");
        }

        [Test]
        public void Can_Parse_Json_With_Nested_Array()
        {
            var selector = "channel.item[*].categories[*]";
            var parser = new JsonObjectParser();
            var jsonString = parser.Parse(JSON_WITH_ARRAYS, selector);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);

            Assert.IsNotNull(result, "result was null");
            Assert.IsNotNull(result.categories, "categories array was null");
            Assert.AreEqual(4, result.categories.Count, "categories length not correct.");
            Assert.AreEqual("Json.NET", result.categories[0], "first categories is not correct.");
            Assert.AreEqual("CodePlex", result.categories[1], "second categories is not correct.");
            Assert.AreEqual("Json.NET", result.categories[2], "third categories is not correct.");
            Assert.AreEqual("LINQ", result.categories[3], "fourth categories is not correct.");
        }


        public void Can_Parse_Json_With_Arrays_And_Properties()
        {
            var selector = "channel.description,channel.link,channel.item[*].title";
            var parser = new JsonObjectParser();
            var jsonString = parser.Parse(JSON_WITH_ARRAYS, selector);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);

            Assert.IsNotNull(result, "result was null");
            Assert.IsNotNull(result.description, "description was null");
            Assert.AreEqual("James Newton - King\'s blog.", result.description, "description is not correct.");
            Assert.IsNotNull(result.link, "link was null");
            Assert.AreEqual("http://james.newtonking.com", result.link, "link is not correct.");
            Assert.AreEqual(2, result.title.Count, "title length not correct.");
            Assert.AreEqual("Json.NET 1.3 + New license + Now on CodePlex", result.title[0], "first title is not correct.");
            Assert.AreEqual("LINQ to JSON beta", result.title[1], "second title is not correct.");
        }



    }
}
