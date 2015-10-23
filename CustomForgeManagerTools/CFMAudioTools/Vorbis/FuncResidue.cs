using System;
using CFMAudioTools.Ogg;

namespace CFMAudioTools.Vorbis 
{
	abstract class FuncResidue
	{
		public static FuncResidue[] residue_P={new Residue0(),
												  new Residue1(),
												  new Residue2()};

		public abstract void pack(Object vr, csBuffer opb);
        public abstract Object unpack(Info vi, csBuffer opb);
        public abstract Object look(DspState vd, InfoMode vm, Object vr);
		public abstract void free_info(Object i);
		public abstract void free_look(Object i);
        public abstract int forward(Block vb, Object vl, float[][] fin, int ch);

        public abstract int inverse(Block vb, Object vl, float[][] fin, int[] nonzero, int ch);
	}
}