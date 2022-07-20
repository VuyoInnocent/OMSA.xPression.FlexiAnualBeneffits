using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace OMSA.Gencorr.Common
{
    public class SerializationHelper
    {
        private const string NAMESPACE_REMOVAL_PATTERN = "<EmailMeta xmlns\\S*=\\S*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))\">";
        private const string NAMESPACE_REMOVAL_REPLACEMENT = "<EmailMeta>";

        /// <summary>
        /// Serialize an object into XML
        /// </summary>
        /// <param name="serializableObject">Object that can be serialized</param>
        /// <returns>Serial XML representation</returns>
        public static string XmlSerialize(object serializableObject)
        {
            XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
            MemoryStream aMemStr = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineChars = "\r\n";
            settings.Encoding = Encoding.GetEncoding(1252);
            //settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.OmitXmlDeclaration = true;

            XmlWriter writer = XmlWriter.Create(aMemStr, settings);

            writer.WriteWhitespace("");  // This gets around the ConformanceLevel.Fragment issue.
            serializer.Serialize(writer, serializableObject);

            //string strXml = Encoding.UTF8.GetString(aMemStr.ToArray());

            Encoding iso = Encoding.GetEncoding(1252);
            //Encoding utf8 = Encoding.UTF8;
            
            //byte[] utfBytes = utf8.GetBytes(strXml);
            //byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);

            String strXml = iso.GetString(aMemStr.ToArray());
            
            strXml = Regex.Replace(strXml, @"xmlns[:xsi|:xsd]*="".*?""", ""); 

            return strXml;
        }

        public static string XmlSerialize(object serializableObject, XmlRootAttribute rootAttribute)
        {
            XmlSerializer serializer = new XmlSerializer(serializableObject.GetType(), rootAttribute);
            MemoryStream aMemStr = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(aMemStr, null);
            serializer.Serialize(writer, serializableObject);
            string strXml = Encoding.UTF8.GetString(aMemStr.ToArray());

            return strXml;
        }

        /// <summary>
        /// Restore (Deserialize) an object, given an XML string
        /// </summary>
        /// <param name="xmlString">XML</param>
        /// <param name="serializableObject">Object to restore as</param>
        public static object XmlDeSerialize(string xmlString, object serializableObject)
        {
            xmlString = Regex.Replace(xmlString, NAMESPACE_REMOVAL_PATTERN, NAMESPACE_REMOVAL_REPLACEMENT);

            XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
            MemoryStream aStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));

            return serializer.Deserialize(aStream);
        }


        /// <summary>
        /// Restore (Deserialize) an object, given an XML string
        /// </summary>
        /// <param name="xmlString">XML</param>
        /// <param name="objectType">Type of object to restore as</param>
        public static object XmlDeSerialize(string xmlString, Type objectType)
        {
            xmlString = Regex.Replace(xmlString, NAMESPACE_REMOVAL_PATTERN, NAMESPACE_REMOVAL_REPLACEMENT);

            XmlSerializer serializer = new XmlSerializer(objectType);
            MemoryStream aStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));

            return serializer.Deserialize(aStream);
        }

        public static object XmlDeSerialize(string xmlString, Type objectType, XmlRootAttribute rootAttribute)
        {
            xmlString = Regex.Replace(xmlString, NAMESPACE_REMOVAL_PATTERN, NAMESPACE_REMOVAL_REPLACEMENT);

            XmlSerializer xmlSerializer = new XmlSerializer(objectType.GetType(), rootAttribute);
            MemoryStream aStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));

            return xmlSerializer.Deserialize(aStream);
        }
    }
}