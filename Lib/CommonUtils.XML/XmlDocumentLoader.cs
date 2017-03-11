using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CommonUtils.XML
{
    public class XmlDocumentLoader
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads XML files from a directory. </summary>
        /// <returns>   The XML files from directory. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static List<XmlDocument> LoadXmlFilesFromDirectory( string directoryPath)
        {
            string[] allFilePaths = Directory.GetFiles(directoryPath);
            var documentList = new List<XmlDocument>();
            foreach (var filePath in allFilePaths)
            {
                string contents = File.ReadAllText(filePath);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(contents);
                documentList.Add(xdoc);
            }
            return documentList;
        }


        public static XmlDocument LoadXmlDocumentFromFile( string file)
        {
            string content = File.ReadAllText(file);
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(content);
            return xdoc;
        }
    }
}
