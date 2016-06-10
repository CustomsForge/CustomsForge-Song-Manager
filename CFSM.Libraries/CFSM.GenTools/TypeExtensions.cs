using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CFSM.GenTools
{
    public static class TypeExtensions
    {
        public static IEnumerable<T> FindAllChildrenByType<T>(this Control control)
        {
            IEnumerable<Control> controls = control.Controls.Cast<Control>();
            return controls.OfType<T>().Concat<T>(controls.SelectMany<Control, T>(ctrl => FindAllChildrenByType<T>(ctrl)));
        }

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

        public static void InitializeClasses(string methodName, Type[] types, object[] objs)
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
                        var pp = t.GetCustomAttributes(typeof (InitPriorityAttribute), false);
                        if (pp.Length > 0)
                        {
                            InitPriorityAttribute p = (InitPriorityAttribute) pp[0];
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
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public static void InitializeClasses(string[] methodNames, Type[] types, object[] objs)
        {
            var bindFlags = BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static;
            var xtypes = GetLoadableTypes().SelectMany<Type, Tuple<Type, MethodInfo, string>>((x, y) =>
                {
                    var lst = new List<Tuple<Type, MethodInfo, string>>();
                    foreach (var s in methodNames)
                    {
                        var mi = (x.GetMethod(s, bindFlags, null, types, null));
                        if (mi != null)
                            lst.Add(new Tuple<Type, MethodInfo, string>(x, mi, s));
                    }
                    return lst.AsEnumerable();
                });

            if (xtypes.Count() > 0)
            {
                Dictionary<string, List<Tuple<int, MethodInfo>>> methodList = new Dictionary<string, List<Tuple<int, MethodInfo>>>();

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
                        var pp = t.Item1.GetCustomAttributes(typeof (InitPriorityAttribute), false);
                        if (pp.Length > 0)
                        {
                            InitPriorityAttribute p = (InitPriorityAttribute) pp[0];
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
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

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

 

}