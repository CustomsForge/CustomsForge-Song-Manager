using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CFSM.GenTools
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
            XmlSerializer serializer = new XmlSerializer(typeof (T));
            serializer.Serialize(stream, obj);
        }

        public static T LoadFromFile<T>(string filePath)
        {
            using (var fs = File.OpenRead(filePath))
                return fs.DeserializeXml<T>();
        }

        public static T DeserializeXml<T>(this Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (T));
            return (T) serializer.Deserialize(stream);
        }

        public static T DeserializeXml<T>(this Stream stream, Type[] types)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (T), types);
            return (T) serializer.Deserialize(stream);
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

        public static System.Xml.XmlDocument XmlSerializeToDom(this object obj)
        {
            var result = new System.Xml.XmlDocument();
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
            return (T) XmlDeserialize(xml, typeof (T));
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
            return (T) XmlDeserializeForClone(XmlSerializeForClone(obj), typeof (T));
        }

        public static void SerializeBin(this object obj, FileStream Stream)
        {
            BinaryFormatter bin = new BinaryFormatter() {FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low};
            bin.Serialize(Stream, obj);
        }

        public static object DeserializeBin(this FileStream Stream)
        {
            object x = null;
            if (Stream.Length > 0)
            {
                BinaryFormatter bin = new BinaryFormatter() {FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low};
                x = bin.Deserialize(Stream);
            }
            return x;
        }
    }
}