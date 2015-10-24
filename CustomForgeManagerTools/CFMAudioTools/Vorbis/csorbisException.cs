using System;

namespace Vorbis 
{
	public class csorbisException : Exception 
	{
        public csorbisException()
            : base() { }
        public csorbisException (String s)
            :base(s){}
	}
}
