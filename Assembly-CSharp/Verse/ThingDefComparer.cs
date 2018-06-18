using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C2D RID: 3117
	public class ThingDefComparer : IEqualityComparer<ThingDef>
	{
		// Token: 0x06004479 RID: 17529 RVA: 0x0023F544 File Offset: 0x0023D944
		public bool Equals(ThingDef x, ThingDef y)
		{
			return (x == null && y == null) || (x != null && y != null && x.shortHash == y.shortHash);
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x0023F590 File Offset: 0x0023D990
		public int GetHashCode(ThingDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04002E73 RID: 11891
		public static readonly ThingDefComparer Instance = new ThingDefComparer();
	}
}
