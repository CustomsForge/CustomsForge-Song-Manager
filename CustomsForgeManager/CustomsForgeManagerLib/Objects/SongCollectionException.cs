using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace CustomsForgeManager.CustomsForgeManagerLib.Objects
{
    [Serializable]
    public class SongCollectionException : Exception
    {
        public SongCollectionException()
        {
            
        }
        public SongCollectionException(string message)
            : base(message)
        {
            
        }
        public SongCollectionException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
        protected SongCollectionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
