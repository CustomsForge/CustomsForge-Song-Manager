
using System;


namespace CFMAudioTools.Vorbis 
{
	abstract class FuncTime
	{
		public static FuncTime[] time_P={new Time0()};

		public abstract void pack(Object i, CFMAudioTools.Ogg.csBuffer opb);
        public abstract Object unpack(Info vi, CFMAudioTools.Ogg.csBuffer opb);
        public abstract Object look(DspState vd, InfoMode vm, Object i);
		public abstract void free_info(Object i);
		public abstract void free_look(Object i);
        public abstract int forward(Block vb, Object i);
        public abstract int inverse(Block vb, Object i, float[] fin, float[] fout);
	}
}