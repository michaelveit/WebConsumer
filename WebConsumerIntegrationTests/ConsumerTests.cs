using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebConsumer.Auth;

namespace WebConsumer.IntegrationTests
{
    [TestFixture]
    public class ConsumerTests
    {
        [Test]
        public void Can_Parse_Xml()
        {
            var url = "https://query.yahooapis.com/v1/public/yql?q=select%20wind%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22chicago%2C%20il%22)&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
            var jsonString = new Consumer().Consume(url, "query.results.channel.wind.chill,query.results.channel.wind.speed");
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
            int val = 0;
            Assert.IsNotNull(result, "result was null");
            Assert.IsNotNull(result.chill, "chill was null.");
            Assert.True(Int32.TryParse(result.chill, out val), "chill was not numeric");
            Assert.IsNotNull(result.speed, "speed was null.");
            Assert.True(Int32.TryParse(result.speed, out val), "speed was not numeric");
        }


        [Test]
        public void Can_Parse_Json()
        {
            var url = "https://query.yahooapis.com/v1/public/yql?q=select%20wind%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22chicago%2C%20il%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
            var jsonString = new Consumer().Consume(url, "query.results.channel.wind.chill,query.results.channel.wind.speed");
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
            int val = 0;

            Assert.IsNotNull(result, "result was null");
            Assert.IsNotNull(result.chill, "chill was null.");
            Assert.True(Int32.TryParse(result.chill, out val), "chill was not numeric");
            Assert.IsNotNull(result.speed, "speed was null.");
            Assert.True(Int32.TryParse(result.speed, out val), "speed was not numeric");
        }

        [Test]
        public void MailChimp()
        {
            var keys = new Dictionary<string, string>();
            keys.Add("apikey", System.Configuration.ConfigurationManager.AppSettings["mailchimpApiKey"]);
            var auth = new AuthorizationHeaderAuth(keys);
            
            var url = "https://us13.api.mailchimp.com/3.0/campaigns/8141a28013";
            var jsonString = new Consumer().Consume(url, "tracking.opens", null, null, auth);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
            bool val = false;

            Assert.IsNotNull(result, "result was null");
            Assert.IsNotNull(result.opens, "opens was null.");
            Assert.That(result.opens is Boolean, "opens was not bool");
        }

        [Test]
        public void CampaignOne()
        {

            var parms = new
            {
                Username = ConfigurationManager.AppSettings["campaignOneUserName"],
                Password = ConfigurationManager.AppSettings["campaignOnePassword"],
                ClientId = Convert.ToInt32(ConfigurationManager.AppSettings["campaignOneClientId"])
            };

            var postParams = JsonConvert.SerializeObject(parms);
           
            var url = "https://app.campaign.one/api/Service/GetEmailList";
            var jsonString = new Consumer().Consume(url, "[*]", "POST", postParams);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
          
            Assert.IsNotNull(result, "result was null");
            Assert.IsNotNull(result.arr, "result array was null.");
            Assert.IsNotNull(result.arr[0].Name, "result name was null.");

        }

        [Test]
        public void CampaignOne_2()
        {

            var parms = new
            {
                Username = ConfigurationManager.AppSettings["campaignOneUserName"],
                Password = ConfigurationManager.AppSettings["campaignOnePassword"],
                ClientId = Convert.ToInt32(ConfigurationManager.AppSettings["campaignOneClientId"])
            };

            var postParams = JsonConvert.SerializeObject(parms);

            var url = "https://app.campaign.one/api/Service/GetEmailList";
            var jsonString = new Consumer().Consume(url, "[*].Name", "POST", postParams);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);

            Assert.IsNotNull(result, "result was null");
            Assert.IsNotNull(result.Name, "result array was null.");
            Assert.IsNotNull(result.Name[0], "result name was null.");

        }

        [Test]
        [Ignore("Service is not working")]
        public void Can_Post_To_CampaignOne()
        {

            var postParams = @"{
  'Username': 'urktest2',
  'Password': 'Password1!',
  'ClientId': 132,
  'TemplateID': '1',
  'Origin': 'string',
  'Forename': 'string',
  'Lastname': 'string',
  'Street': 'string',
  'Zip': '11111',
  'City': 'string',
  'Telephone1': '1234567',
  'Telephone2': '3456789',
  'Telephone3': '5671234',
  'Telephone4': '7890123',
  'Email': 'name@domin.com',
  'Country': 'string',
  'CountryCode': '0045',
  'CustomerNr': 'string',
  'Companyname': 'string',
  'Gender': 'string',
  'CustomerStatus': 'string',
  'ExtFields': 'field1=value1;field2=value2;field3=value3'
}";

            var url = "https://app.campaign.one/api/Service/AddCustomer";
            var jsonString = new Consumer().Consume(url, "", "POST", postParams);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);

            Assert.IsNotNull(result, "result was null");
            //Assert.IsNotNull(result.arr, "result array was null.");
            //Assert.IsNotNull(result.arr[0].Name, "result name was null.");

        }


        [Test]
        [Ignore("Do not have real credentials")]
        public void Actimizer()
        {
            var keys = new Dictionary<string, string>();
            keys.Add("basic", System.Configuration.ConfigurationManager.AppSettings["actimizerauth"]);
            var auth = new AuthorizationHeaderAuth(keys);

            var url = "https://wsdk.actimizer.com/api/leads";
            var jsonString = new Consumer().Consume(url, "LeadForExport.uniqueId", null, null, auth);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
            
            Assert.IsNotNull(result, "result was null");
            Assert.IsNotNull(result.LeadForExport.uniqueId, "LeadForExport was null.");
        }

    }
}
