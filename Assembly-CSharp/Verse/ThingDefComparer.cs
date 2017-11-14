using System.Collections.Generic;

namespace Verse
{
	public class ThingDefComparer : IEqualityComparer<ThingDef>
	{
		public static readonly ThingDefComparer Instance = new ThingDefComparer();

		public bool Equals(ThingDef x, ThingDef y)
		{
			if (x == null && y == null)
			{
				return true;
			}
			if (x != null && y != null)
			{
				return x.shortHash == y.shortHash;
			}
			return false;
		}

		public int GetHashCode(ThingDef obj)
		{
			return obj.GetHashCode();
		}
	}
}
