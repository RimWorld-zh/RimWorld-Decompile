using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C2B RID: 3115
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		// Token: 0x04002E7C RID: 11900
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();

		// Token: 0x06004481 RID: 17537 RVA: 0x002409A0 File Offset: 0x0023EDA0
		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x002409BC File Offset: 0x0023EDBC
		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}
	}
}
