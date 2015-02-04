﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomsForgeManager_Winforms.Utilities
{
    public static class Extensions
    {
        public static void InvokeIfRequired<T>(this T c, Action<T> action) where T : Control
        {
            if (c.InvokeRequired)
                c.Invoke(new Action(() => action(c)));
            else
                action(c);
        }
        public static void Serialze(object obj, FileStream Stream)
        {
            BinaryFormatter bin = new BinaryFormatter();
            bin.FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low;
            bin.Serialize(Stream, obj);
        }
        public static object DeSerialize(FileStream Stream)
        {
            BinaryFormatter bin = new BinaryFormatter();
            bin.FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low;
            object x = bin.Deserialize(Stream);
            return x;
        }
    }
}
