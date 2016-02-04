using System;
using System.Collections.Generic;
using System.Text;
using Antlr.Runtime;

namespace CFSM.NCalc
{
    public class DFA_X
    {
        public static short[][] UnpackEncodedStringArray(string[] encodedStrings)
        {
            short[][] array = new short[encodedStrings.Length][];
            for (int i = 0; i < encodedStrings.Length; i++)
            {
                array[i] = DFA.UnpackEncodedString(encodedStrings[i]);
            }
            return array;
        }
    }

    public class StackList : List<object>
    {
        public void Push(object item)
        {
            base.Add(item);
        }

        public object Pop()
        {
            object result = base[base.Count - 1];
            base.RemoveAt(base.Count - 1);
            return result;
        }

        public object Peek()
        {
            return base[base.Count - 1];
        }
    }
}