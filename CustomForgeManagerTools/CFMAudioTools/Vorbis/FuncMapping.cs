using System;


namespace CFMAudioTools.Vorbis 
{
	abstract class FuncMapping
	{
		public static FuncMapping[] mapping_P={new Mapping0()};

        public abstract void pack(Info info, Object imap, CFMAudioTools.Ogg.csBuffer buffer);
        public abstract Object unpack(Info info, CFMAudioTools.Ogg.csBuffer buffer);
        public abstract Object look(DspState vd, InfoMode vm, Object m);
		public abstract void free_info(Object imap);
		public abstract void free_look(Object imap);
        public abstract int inverse(Block vd, Object lm);
	}
}