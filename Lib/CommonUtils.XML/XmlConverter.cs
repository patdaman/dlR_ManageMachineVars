using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;

namespace CommonUtils.XML
{
    public class XmlConverter
    {
        public static string XmlToJson(XmlDocument XmlMessage)
        {
            string jsonText = JsonConvert.SerializeXmlNode(XmlMessage);

            /* Make Everything an Array */
            jsonText = Regex.Replace(jsonText, "},\"", "}],\"");
            jsonText = Regex.Replace(jsonText, "\":\\{", "\":[{");
            jsonText = jsonText.Replace("\\", "");
            jsonText = Regex.Replace(jsonText, @"}}", "}]}");
            jsonText = Regex.Replace(jsonText, @"}}", "}]}");
            return jsonText;
        }

        public static T XmlToObjectJSON<T>(XmlDocument XmlMessage)
        {
            T message = default(T);
            string jsonText = XmlToJson(XmlMessage);
            JObject jsonResult = (JObject)JsonConvert.DeserializeObject(jsonText);
            message = jsonResult.ToObject<T>();
            return message;
        }

        public static T XmlToObject<T>(XmlDocument XmlMessage)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = XmlReader.Create(new StringReader(XmlMessage.OuterXml.ToString())))
            {
                T message = (T)serializer.Deserialize(reader);
                return message;
            }
        }

        public static XmlDocument AddJsonNetRootAttribute(XmlDocument xmlDoc)
        {
            XmlAttribute jsonNS = xmlDoc.CreateAttribute("xmlns", "json", @"http://www.w3.org/2000/xmlns/");
            jsonNS.Value = @"http://james.newtonking.com/projects/json";

            xmlDoc.DocumentElement.SetAttributeNode(jsonNS);
            return xmlDoc;
        }

        private static XmlDocument AddJsonArrayAttributesForXPath(string xpath, XmlDocument xmlDoc)
        {
            XmlNodeList nodeList;
            XmlNode root = xmlDoc.DocumentElement;
            nodeList = root.SelectNodes(xpath);
            foreach (var element in nodeList)
            {
                var el = element as XmlElement;
                if (el != null)
                {
                    var jsonArray = xmlDoc.CreateAttribute("json", "Array", @"http://james.newtonking.com/json");
                    jsonArray.Value = "true";
                    el.SetAttributeNode(jsonArray);
                }
            }
            return xmlDoc;
        }
    }
}
