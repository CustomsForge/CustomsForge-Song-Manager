
using System;
using Ogg;

namespace Vorbis 
{
	abstract class FuncFloor
	{
		public static FuncFloor[] floor_P={new Floor0(),new Floor1()};

		public abstract void pack(Object i, csBuffer opb);
        public abstract Object unpack(Info vi, csBuffer opb);
        public abstract Object look(DspState vd, InfoMode mi, Object i);
		public abstract void free_info(Object i);
		public abstract void free_look(Object i);
		public abstract void free_state(Object vs);
        public abstract int forward(Block vb, Object i, float[] fin, float[] fout, Object vs);
        public abstract Object inverse1(Block vb, Object i, Object memo);
        public abstract int inverse2(Block vb, Object i, Object memo, float[] fout);
	}
}