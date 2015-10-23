using System;

namespace CFMAudioTools
{
    public class InvalidIDException : Exception
    {
        public InvalidIDException()
        {

        }
        public InvalidIDException(uint message)
            : base("invalid codebook identifier: " + message)
        {

        }
        public InvalidIDException(string message)
            : base(message)
        {

        }
        public InvalidIDException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        protected InvalidIDException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}