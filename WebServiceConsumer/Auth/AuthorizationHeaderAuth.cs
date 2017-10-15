using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebConsumer.Auth
{
    public class AuthorizationHeaderAuth : IAuthorization
    {
        private IDictionary<string, string> _keys;

        public AuthorizationHeaderAuth(IDictionary<string, string> keys)
        {
            _keys = keys;
        }

        public void Apply(HttpRequestMessage message)
        {

            var kv = _keys.First();
            var value = kv.Value;

            if (kv.Key == "Basic")
                value = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(kv.Value));
           
            message.Headers.Authorization
                        = new AuthenticationHeaderValue(kv.Key, value); 

        }
    }
}
