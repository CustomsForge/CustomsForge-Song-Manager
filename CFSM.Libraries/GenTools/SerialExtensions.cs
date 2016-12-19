using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace GenTools
{
    public static class SerialExtensions
    {
        public static void SaveToFile<T>(string filePath, T obj)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                obj.SerializeXml(fs);
        }

        public static void SerializeXml<T>(this T obj, Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, obj);
        }

        public static T LoadFromFile<T>(string filePath)
        {
            using (var fs = File.OpenRead(filePath))
                return fs.DeserializeXml<T>();
        }

        public static T DeserializeXml<T>(this Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }

        public static T DeserializeXml<T>(this Stream stream, Type[] types)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), types);
            return (T)serializer.Deserialize(stream);
        }

        public static string XmlSerialize(this object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            using (StreamReader reader = new StreamReader(memoryStream))
                return reader.ReadToEnd();
        }

        public static XmlDocument XmlSerializeToDom(this object obj)
        {
            var result = new XmlDocument();
            result.LoadXml(XmlSerialize(obj));
            return result;
        }

        public static object XmlDeserialize(string xml, Type toType)
        {
            using (Stream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(toType);
                using (TextReader tr = new StringReader(xml))
                    return serializer.Deserialize(tr);
            }
        }

        public static T XmlDeserialize<T>(string xml)
        {
            return (T)XmlDeserialize(xml, typeof(T));
        }

        public static string XmlSerializeForClone(object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                //   serializer.PreserveObjectReferences = false;
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static object XmlDeserializeForClone(string xml, Type toType)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;

                DataContractSerializer deserializer = new DataContractSerializer(toType);
                return deserializer.ReadObject(stream);
            }
        }

        public static T XmlClone<T>(this T obj)
        {
            return (T)XmlDeserializeForClone(XmlSerializeForClone(obj), typeof(T));
        }

        public static void SaveToBinFile<T>(string filePath, T obj)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                obj.SerializeBin(fs);
        }

        public static void SerializeBin(this object obj, FileStream Stream)
        {
            BinaryFormatter bin = new BinaryFormatter() { FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low };
            bin.Serialize(Stream, obj);
        }

        public static T LoadFromBinFile<T>(string filePath)
        {
            using (var fs = File.OpenRead(filePath))
                return fs.DeserializeBin<T>();
        }

        public static T DeserializeBin<T>(this FileStream Stream)
        {
            if (Stream.Length > 0)
            {
                BinaryFormatter bin = new BinaryFormatter() { FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low };
                return (T)bin.Deserialize(Stream);
            }
            return default(T);
        }

        public static List<string> DeserializeAttribute(string filePath, string nodeName, string attributeName)
        {
            var attributesList = new List<string>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNodeList nodes = doc.SelectNodes(nodeName);

                foreach (XmlNode node in nodes)
                    if (node.Attributes[attributeName] != null)
                        attributesList.Add(node.Attributes[attributeName].Value);
            }
            catch (XmlException ex)
            {
                throw new XmlException("Invalid XML file:" + filePath + Environment.NewLine + ex.Message);
            }
            catch (IOException ex)
            {
                throw new IOException("Unable to access file: " + filePath + Environment.NewLine + ex.Message);
            }


            return attributesList;
        }

        public static void SaveSetttings<T>(string filePath, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter textWriter = new StreamWriter(filePath);
            serializer.Serialize(textWriter, obj);
            textWriter.Close();
        }

        public static T LoadSettings<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextReader reader = new StreamReader(filePath);
            var data = (T)serializer.Deserialize(reader);
            reader.Close();

            return data;
        }

        /// <summary>
        /// Reads the data of specified node provided in the parameter
        /// </summary>
        /// <param name="nodeToRead">Node to be read</param>
        /// <returns>string containing the value</returns>
        private static string ReadValueFromXML(string filePath, string nodeToRead)
        {
            try
            {
                //settingsFilePath is a string variable storing the path of the settings file 
                XPathDocument doc = new XPathDocument(filePath);
                XPathNavigator nav = doc.CreateNavigator();
                // Compile a standard XPath expression
                XPathExpression expr;
                expr = nav.Compile(@"/settings/" + nodeToRead);
                XPathNodeIterator iterator = nav.Select(expr);
                // Iterate on the node set
                while (iterator.MoveNext())
                {
                    return iterator.Current.Value;
                }
                return string.Empty;
            }
            catch (XmlException ex)
            {
                throw new XmlException("Invalid XML file:" + filePath + Environment.NewLine + ex.Message);
            }
            catch (IOException ex)
            {
                throw new IOException("Unable to access file: " + filePath + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Writes the updated value to XML
        /// </summary>
        /// <param name="nodeToRead">Node of XML to read</param>
        /// <param name="valueToWrite">Value to write to that node</param>
        /// <returns></returns>
        private static bool WriteValueToXML(string filePath, string nodeToRead, string valueToWrite)
        {
            try
            {
                //settingsFilePath is a string variable storing the path of the settings file 
                XmlTextReader reader = new XmlTextReader(filePath);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                //we have loaded the XML, so it's time to close the reader.
                reader.Close();
                XmlNode oldNode;
                XmlElement root = doc.DocumentElement;
                oldNode = root.SelectSingleNode("/settings/" + nodeToRead);
                oldNode.InnerText = valueToWrite;
                doc.Save(filePath);
                return true;
            }
            catch (XmlException ex)
            {
                throw new XmlException("Invalid XML file:" + filePath + Environment.NewLine + ex.Message);
            }
            catch (IOException ex)
            {
                throw new IOException("Unable to access file: " + filePath + Environment.NewLine + ex.Message);
            }
        }

        public static void SerializeCustomXml<T>(this T obj, Stream stream, bool omitEncoding = false, bool omitNamespace = true, bool customDocType = false)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "   ";
            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.OmitXmlDeclaration = omitEncoding; // removes <?xml version
            settings.Encoding = new UTF8Encoding(false);

            using (var writer = XmlWriter.Create(stream, settings))
            {
                if (customDocType)
                {
                    writer.WriteStartDocument();
                    writer.WriteDocType(DocTypeName, DocTypePubID, DocTypeSysID, null);
                }

                if (omitNamespace)
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    new XmlSerializer(typeof(T)).Serialize(writer, obj, ns);
                }
                else
                    new XmlSerializer(typeof(T)).Serialize(writer, obj);
            }

            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
        }

        // poperly encodes with utf-8 or others and does not show header <?xml version="1.0" encoding="utf-8"?>
        public static string SerializeToString<T>(T xmlObj, Encoding encoding = null, bool omitEncoding = true, bool omitNamespace = true, bool customHeader = false)
        {
            if (xmlObj == null) return null;
            if (encoding == null)
                encoding = Encoding.UTF8;

            var xmlSerializer = new XmlSerializer(xmlObj.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "   ";
            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.OmitXmlDeclaration = omitEncoding; // removes <?xml version
            settings.Encoding = new UTF8Encoding(false);

            using (var xmlTextWriter = new StringWriterWithEncoding(encoding))
            using (var writer = XmlWriter.Create(xmlTextWriter, settings))
            {
                if (customHeader)
                {
                    writer.WriteStartDocument();
                    writer.WriteDocType(DocTypeName, DocTypePubID, DocTypeSysID, null);
                }

                if (omitNamespace)
                {
                    XmlSerializerNamespaces xs = new XmlSerializerNamespaces();
                    xs.Add("", "");
                    xmlSerializer.Serialize(writer, xmlObj, xs);
                }
                else
                    xmlSerializer.Serialize(writer, xmlObj);

                return xmlTextWriter.ToString();
            }
        }

        public static void SerializeToFile<T>(string xmlDestPath, T xmlObj, Encoding encoding = null, bool omitDeclaration = true, bool customHeader = false)
        {
            File.WriteAllText(xmlDestPath, SerializeToString<T>(xmlObj, encoding, omitDeclaration, customHeader));
        }

        public static T DeserializeFromString<T>(string xmlString) where T : new()
        {
            T xmlObj = new T();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader sr = new StringReader(xmlString);
            xmlObj = (T)xmlSerializer.Deserialize(sr);
            return xmlObj;
        }

        public static T DeserializeFromFile<T>(string xmlSrcPath) where T : new()
        {
            if (!File.Exists(xmlSrcPath))
                throw new FileNotFoundException();

            var xmlText = File.ReadAllText(xmlSrcPath);

            // remove invalid xml hex and unicode characters
            var invalidXml = @"&#x?[^;]{2,4};"; // &#x1E; &#x0007;
            xmlText = Regex.Replace(xmlText, invalidXml, "", RegexOptions.Compiled);

            //var invalidXml = "&#x1E;"; // &#x1E;&amp;          
            //xmlText = Regex.Replace(xmlText, invalidXml, "", RegexOptions.Compiled);

            //// to be valid '&' must be used like  '&amp;' in xml
            //var badHex = "[\x00-\x08\x0B\x0C\x0E-\x1F]"; // \x26]"; &#x1E;&amp;            
            //xmlText = Regex.Replace(xmlText, badHex, "", RegexOptions.Compiled);

            return DeserializeFromString<T>(xmlText);
        }

        private static string _docTypeName;
        [XmlIgnore]
        public static string DocTypeName
        {
            get
            {
                if (_docTypeName == null)
                    _docTypeName = "settings-xml";
                return _docTypeName;
            }
            set { }
        }

        private static string _docTypePubID;
        [XmlIgnore]
        public static string DocTypePubID
        {
            get
            {
                if (_docTypePubID == null)
                    _docTypePubID = "-//Cozy1//XML Settings//EN";
                return _docTypePubID;
            }
            set { }
        }

        private static string _docTypeSysID;
        [XmlIgnore]
        public static string DocTypeSysID
        {
            get
            {
                if (_docTypeSysID == null)
                    _docTypeSysID = "http://www.cozy1.com";
                return _docTypeSysID;
            }
            set { }
        }
    }

    // this is required for StringWriter encoding work around
    internal class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding encoding;

        public StringWriterWithEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }


}