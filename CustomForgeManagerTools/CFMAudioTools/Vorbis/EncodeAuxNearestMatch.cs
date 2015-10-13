using System;

namespace CFMAudioTools.Vorbis 
{

	class EncodeAuxNearestMatch
	{
		internal int[] ptr0;
		internal int[] ptr1;

		internal int[] p;         // decision points (each is an entry)
		internal int[] q;         // decision points (each is an entry)
		internal int   aux;       // number of tree entries
		internal int   alloc;       
	}
}
