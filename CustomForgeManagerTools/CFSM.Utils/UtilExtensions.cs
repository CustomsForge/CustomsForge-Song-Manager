using System.Runtime.Serialization.Formatters.Binary;
using CFSM.Utils.PSARC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CFSM.Utils
{
    public static class UtilExtensions
    {
        #region Types
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IEnumerable<Type> GetLoadableTypes()
        {
            List<Type> allTypes = new List<Type>();
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                allTypes.AddRange(GetLoadableTypes(ass));
            }
            return allTypes;
        }

        public static IEnumerable<Type> GetTypesAssignableFrom(this Assembly asm, Type AType)
        {
            return GetLoadableTypes(asm).Where(AType.IsAssignableFrom).ToList();
        }

        public static IEnumerable<T> FindAllChildrenByType<T>(this Control control)
        {
            IEnumerable<Control> controls = control.Controls.Cast<Control>();
            return controls
                .OfType<T>()
                .Concat<T>(controls.SelectMany<Control, T>(ctrl => FindAllChildrenByType<T>(ctrl)));
        }

        public static void InitializeClasses(string methodName, Type[] types ,object[] objs)
        {
            var bindFlags = BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static;
            var xtypes = GetLoadableTypes().Where(at => at.GetMethod(methodName, bindFlags, null, types, null) != null);            
            if (xtypes.Count() > 0)
            {
                List<Tuple<int, MethodInfo>> funcList = new List<Tuple<int, MethodInfo>>();
                foreach (var t in xtypes)
                {
                    var meth = t.GetMethod(methodName, bindFlags, null, types, null);
                    if (meth != null)
                    {
                        int priorty = 0;
                        var pp = t.GetCustomAttributes(typeof(InitPriorityAttribute), false);
                        if (pp.Length > 0)
                        {
                            InitPriorityAttribute p = (InitPriorityAttribute)pp[0];
                            priorty = p.Priority;
                        }
                        funcList.Add(new Tuple<int, MethodInfo>(priorty, meth));
                    }
                }
                funcList.Sort((s1, s2) =>
                {
                    if (s1.Item1 > s2.Item1)
                        return -1;
                    if (s1.Item1 > s2.Item1)
                        return -1;
                    return 0;
                });
                foreach (Tuple<int, MethodInfo> mi in funcList)
                {
                    try
                    {
                        mi.Item2.Invoke(null, objs);
                    }
                    catch (Exception){  }
                }
            }
        }


        public static void InitializeClasses(string[] methodNames, Type[] types, object[] objs)
        {
            var bindFlags = BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static;
            var xtypes = GetLoadableTypes().
                SelectMany<Type, Tuple<Type, MethodInfo, String>>((x, y) =>
                {
                    var lst = new List<Tuple<Type, MethodInfo, String>>();
                    foreach (var s in methodNames)
                    {
                        var mi = (x.GetMethod(s, bindFlags, null, types, null));
                        if (mi != null)
                            lst.Add(new Tuple<Type, MethodInfo, String>(x, mi,s));
                    }
                    return lst.AsEnumerable();
                });


            if (xtypes.Count() > 0)
            {
                Dictionary<String, List<Tuple<int, MethodInfo>>> methodList = new Dictionary<string, List<Tuple<int, MethodInfo>>>();

                foreach (var t in xtypes)
                {
                    if (!methodList.ContainsKey(t.Item3))
                    {
                        methodList.Add(t.Item3, new List<Tuple<int, MethodInfo>>());
                    }
                    var funcList = methodList[t.Item3];

                    var meth = t.Item2;
                    if (meth != null)
                    {
                        int priorty = 0;
                        var pp = t.Item1.GetCustomAttributes(typeof(InitPriorityAttribute), false);
                        if (pp.Length > 0)
                        {
                            InitPriorityAttribute p = (InitPriorityAttribute)pp[0];
                            priorty = p.Priority;
                        }
                        funcList.Add(new Tuple<int, MethodInfo>(priorty, meth));
                    }
                }

                foreach (var item in methodList)
                {
                    item.Value.Sort((s1, s2) =>
                    {
                        if (s1.Item1 > s2.Item1)
                            return -1;
                        if (s1.Item1 > s2.Item1)
                            return -1;
                        return 0;
                    });
                    foreach (Tuple<int, MethodInfo> mi in item.Value)
                    {
                        try
                        {
                            mi.Item2.Invoke(null, objs);
                        }
                        catch (Exception) { }
                    }
                }

            }
        }

        #endregion
  
        #region Serialization
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


        public static T DeserializeXml<T>(this Stream stream,Type[] types)
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

        public static void SerializeBin(this object obj, FileStream Stream)
        {
            BinaryFormatter bin = new BinaryFormatter()
            {
                FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low
            };
            bin.Serialize(Stream, obj);
        }

        public static object DeserializeBin(this FileStream Stream)
        {
            object x = null;
            if (Stream.Length > 0)
            {
                BinaryFormatter bin = new BinaryFormatter()
                {
                    FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low
                };
                x = bin.Deserialize(Stream);
            }
            return x;
        }

        #endregion

       

        #region Directory Methods
        public static void DeleteEmptyDirs(this DirectoryInfo dir)
        {
            foreach (DirectoryInfo d in dir.GetDirectories())
                d.DeleteEmptyDirs();

            try
            {
                dir.Delete();
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static void TempChangeDirectory(String directory, Action act)
        {
            String old = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(directory);
                act();
            }
            finally
            {
                Directory.SetCurrentDirectory(old);
            }
        }
        
        #endregion

        #region PSARC
        public static bool ReplaceData(this CFSM.Utils.PSARC.PSARC p,
          Func<Entry, bool> dataEntry,
          Stream newData)
        {
            var de = p.TOC.Where(dataEntry).FirstOrDefault();
            if (de != null)
            {
                if (de.Data != null)
                {
                    de.Data.Dispose();
                    de.Data = null;
                }
                else
                    p.InflateEntry(de);

                de.Data = newData;
                return true;
            }
            return false;
        }

        public static Stream ExtractPSARCData(this Stream stream, Func<Entry, bool> dataEntry)
        {
            using (CFSM.Utils.PSARC.PSARC p = new PSARC.PSARC(true))
            {
                p.Read(stream, true);

                var de = p.TOC.Where(dataEntry).FirstOrDefault();
                if (de != null)
                {
                    MemoryStream ms = new MemoryStream();
                    p.InflateEntry(de);
                    if (de.Data == null)
                        return null;
                    de.Data.Position = 0;
                    de.Data.CopyTo(ms);
                    ms.Position = 0;
                    return ms;
                }
                return null;
            }
        }


        public static Stream GetData(this CFSM.Utils.PSARC.PSARC p,
          Func<Entry, bool> dataEntry)
        {
             var de = p.TOC.Where(dataEntry).FirstOrDefault();
             if (de != null)
             {
                 if (de.Data == null)
                     p.InflateEntry(de);

                 return de.Data;
             }
             return null;
        }

        public static bool ReplaceData(this CFSM.Utils.PSARC.PSARC p, Dictionary<Func<Entry, bool>, Stream> newData)
        {
            bool result = true;
            foreach (var d in newData)
            {
                if (!p.ReplaceData(d.Key, d.Value))
                    result = false;
            }
            return result;
        }

        public static NoCloseStream ReplaceData(this CFSM.Utils.PSARC.PSARC p,
            Func<Entry, bool> dataEntry, String newData)
        {
            NoCloseStream s = new NoCloseStream();
            using (var sr = new StreamWriter(s))
                sr.Write(newData);
            s.Position = 0;
            if (!ReplaceData(p, dataEntry, s))
            {
                s.canClose = true;
                s.Dispose();
                return null;
            }
            return s;
        } 
        #endregion
    }


    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class InitPriorityAttribute : Attribute
    {
        public int Priority;
        public InitPriorityAttribute(int Priority)
        {
            this.Priority = Priority;
        }
    }
    /// <summary>
    /// Useful when some other class(StreamReader/StreamReader...) tries to close the stream before it's supposed to be closed.
    /// </summary>
    public class NoCloseStream : MemoryStream
    {
        public bool canClose = false;

        public void CloseEx()
        {
            canClose = true;
            this.Close();
        }

        public override void Close()
        {
            if (canClose)
                base.Close();
        }
    }

    public class NoCloseStreamList : List<NoCloseStream>, IDisposable
    {

        public NoCloseStream NewStream()
        {
            var l = new NoCloseStream();
            Add(l);
            return l;
        }

        public void Dispose()
        {
            foreach (var l in this)
                l.CloseEx();
            this.Clear();
        }
    }

}
