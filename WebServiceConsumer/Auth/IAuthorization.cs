using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebConsumer.Auth
{
    public interface IAuthorization
    {
        void Apply(HttpRequestMessage message);
    }
}
