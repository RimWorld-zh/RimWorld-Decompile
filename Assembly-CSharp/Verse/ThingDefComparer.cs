using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C2E RID: 3118
	public class ThingDefComparer : IEqualityComparer<ThingDef>
	{
		// Token: 0x0600447B RID: 17531 RVA: 0x0023F56C File Offset: 0x0023D96C
		public bool Equals(ThingDef x, ThingDef y)
		{
			return (x == null && y == null) || (x != null && y != null && x.shortHash == y.shortHash);
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x0023F5B8 File Offset: 0x0023D9B8
		public int GetHashCode(ThingDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04002E75 RID: 11893
		public static readonly ThingDefComparer Instance = new ThingDefComparer();
	}
}
