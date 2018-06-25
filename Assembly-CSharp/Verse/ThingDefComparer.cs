using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C2D RID: 3117
	public class ThingDefComparer : IEqualityComparer<ThingDef>
	{
		// Token: 0x04002E84 RID: 11908
		public static readonly ThingDefComparer Instance = new ThingDefComparer();

		// Token: 0x06004485 RID: 17541 RVA: 0x00240CC8 File Offset: 0x0023F0C8
		public bool Equals(ThingDef x, ThingDef y)
		{
			return (x == null && y == null) || (x != null && y != null && x.shortHash == y.shortHash);
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x00240D14 File Offset: 0x0023F114
		public int GetHashCode(ThingDef obj)
		{
			return obj.GetHashCode();
		}
	}
}
