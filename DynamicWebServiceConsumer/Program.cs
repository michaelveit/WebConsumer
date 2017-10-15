using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebConsumer;

namespace WebConsumer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "https://query.yahooapis.com/v1/public/yql?q=select%20wind%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22chicago%2C%20il%22)&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
            var result = new Consumer().Consume(url, "query.results.channel.wind.chill,query.results.channel.wind.speed");
            System.Diagnostics.Debug.Print(result);
        }
    }
}
