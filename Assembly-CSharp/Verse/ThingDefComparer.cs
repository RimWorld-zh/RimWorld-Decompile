using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C2C RID: 3116
	public class ThingDefComparer : IEqualityComparer<ThingDef>
	{
		// Token: 0x04002E7D RID: 11901
		public static readonly ThingDefComparer Instance = new ThingDefComparer();

		// Token: 0x06004485 RID: 17541 RVA: 0x002409E8 File Offset: 0x0023EDE8
		public bool Equals(ThingDef x, ThingDef y)
		{
			return (x == null && y == null) || (x != null && y != null && x.shortHash == y.shortHash);
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x00240A34 File Offset: 0x0023EE34
		public int GetHashCode(ThingDef obj)
		{
			return obj.GetHashCode();
		}
	}
}
