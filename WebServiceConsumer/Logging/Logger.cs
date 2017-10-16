using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConsumer.Logging
{
    public static class Logger
    {

       

        private static readonly log4net.ILog log
                = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static Logger() 
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void LogRequest(string request)
        {
            string message = "Request: " + request;
            WriteLog(message);
        }

        public static void LogResponse(string response)
        {
            string message = "Response: " + response;
            WriteLog(message);
        }

        public static void LogException(Exception ex)
        {
            WriteLog("An error occurred: " + ex);
        }


        public static void WriteLog(string message, Exception ex = null)
        {
            if (ex == null)
                log.Debug(message);
            else
                log.Error(message, ex);
        }
    }
}
