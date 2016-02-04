using System;

namespace CFSM.AudioTools.Vorbis
{
    public class csorbisException : Exception
    {
        public csorbisException() : base()
        {
        }

        public csorbisException(String s) : base(s)
        {
        }
    }
}