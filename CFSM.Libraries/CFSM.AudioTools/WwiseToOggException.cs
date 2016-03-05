using System;

namespace CFSM.AudioTools
{
    public class WwiseToOggException : Exception
    {
        public WwiseToOggException()
        {
        }

        public WwiseToOggException(string message) : base(message)
        {
        }

        public WwiseToOggException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WwiseToOggException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}