using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConsumer
{
    public class JsonObjectParser
    {
        
        public JsonObjectParser()
        {
        }

        
        public string Parse(string json, string selector)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new Exception("JSON string was empty");
            }

            if (string.IsNullOrWhiteSpace(selector))
            {
                return json;
            }

            var result = new JObject();
            var jsonObj = JObject.Parse(json);
            // parse templates
            var paths = selector.Split(',');
            foreach(var path in paths.Select(x => x.Trim()))
            {
                if (path.Contains("[*]"))
                {
                    //Process array
                    var values = jsonObj.SelectTokens(path);
                    AssertHasValue(values, path);
                    var jArray = new JArray();
                    foreach (var jt in values)
                    {
                        jArray.Add(jt);
                    }
                    result[GetPropertyName(path)] = jArray;
                }
                else
                {
                    var value = jsonObj.SelectToken(path);
                    AssertHasValue(value, path);
                    result.Add(GetPropertyName(path), value);
                }
            }

            return result.ToString(Formatting.None);
        }

        private string GetPropertyName(string path)
        {
            return path.Replace("[*]", "").Split('.').Last();
        }

        private void AssertHasValue(IEnumerable<JToken> value, string path)
        {
            if (!value.Any())
                throw BadPathException(path); 
        }

        private void AssertHasValue(JToken value, string path)
        {
            if (value == null)
                throw BadPathException(path);
        }

        private Exception BadPathException(string path)
        {
            return new Exception("The selector path " + path + " was not found."); 
        }
    }

}
