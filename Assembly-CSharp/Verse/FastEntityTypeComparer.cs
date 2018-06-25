using System;
using System.Collections.Generic;

namespace Verse
{
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();

		public FastEntityTypeComparer()
		{
		}

		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static FastEntityTypeComparer()
		{
		}
	}
}
