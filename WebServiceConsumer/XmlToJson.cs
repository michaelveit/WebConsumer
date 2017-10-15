using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace WebConsumer
{
    public static class XmlToJson
    {
        public static string Convert(string xml)
        {
            var doc = XDocument.Parse(xml);
            doc.Declaration = null;
            foreach (XElement XE in doc.Root.DescendantsAndSelf())
            {
                XE.Name = XE.Name.LocalName;
                XE.ReplaceAttributes((from xattrib in XE.Attributes().Where(xa => !xa.IsNamespaceDeclaration) select new XAttribute(xattrib.Name.LocalName, xattrib.Value)));
            }
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(doc.ToString());
            var json = JsonConvert.SerializeXmlNode(xmlDocument);
            return (Regex.Replace(json, "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase));
        }
    }
}
