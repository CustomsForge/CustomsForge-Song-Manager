using System;

namespace CFMAudioTools.Vorbis 
{
	public class csorbisException : Exception 
	{
        public csorbisException()
            : base() { }
        public csorbisException (String s)
            :base(s){}
	}
}
