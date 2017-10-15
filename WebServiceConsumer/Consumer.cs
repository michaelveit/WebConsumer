using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebConsumer.Auth;

namespace WebConsumer
{
    public class Consumer
    {
        public string Consume(string url, string template, IAuthorization auth = null)
        {
            var requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
            };
          
            if (auth != null)
            {
                auth.Apply(requestMessage);
            }

            using (var client = new HttpClient())
            {
                var response = client.SendAsync(requestMessage).Result;
                var statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                var media = response.Content.Headers.ContentType.MediaType;
                var stream = response.Content.ReadAsStreamAsync().Result;

                var payload = string.Empty;
                using (var reader = new StreamReader(stream))
                {
                    payload = reader.ReadToEnd();
                }
                if (media.Contains("xml"))
                {
                    payload = XmlToJson.Convert(payload);
                }
                var parser = new JsonObjectParser();
                return parser.Parse(payload, template);
            }
        }

    }
}
