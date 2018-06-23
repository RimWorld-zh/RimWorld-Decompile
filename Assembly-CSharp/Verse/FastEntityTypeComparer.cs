using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C29 RID: 3113
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		// Token: 0x04002E7C RID: 11900
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();

		// Token: 0x0600447E RID: 17534 RVA: 0x002408C4 File Offset: 0x0023ECC4
		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x002408E0 File Offset: 0x0023ECE0
		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}
	}
}
