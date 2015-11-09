using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomsForgeManager.CustomsForgeManagerLib.Objects
{
    class RSDataJsonDictionary<T> : Dictionary<string, Dictionary<string, T>> where T : RSDataAbstractBase
    {
    }
}
