using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C2A RID: 3114
	public class ThingDefComparer : IEqualityComparer<ThingDef>
	{
		// Token: 0x06004482 RID: 17538 RVA: 0x0024090C File Offset: 0x0023ED0C
		public bool Equals(ThingDef x, ThingDef y)
		{
			return (x == null && y == null) || (x != null && y != null && x.shortHash == y.shortHash);
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x00240958 File Offset: 0x0023ED58
		public int GetHashCode(ThingDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04002E7D RID: 11901
		public static readonly ThingDefComparer Instance = new ThingDefComparer();
	}
}
