using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C2D RID: 3117
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		// Token: 0x06004477 RID: 17527 RVA: 0x0023F524 File Offset: 0x0023D924
		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x0023F540 File Offset: 0x0023D940
		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}

		// Token: 0x04002E74 RID: 11892
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();
	}
}
