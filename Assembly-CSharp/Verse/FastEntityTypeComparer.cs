using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C2C RID: 3116
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		// Token: 0x04002E83 RID: 11907
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();

		// Token: 0x06004481 RID: 17537 RVA: 0x00240C80 File Offset: 0x0023F080
		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x00240C9C File Offset: 0x0023F09C
		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}
	}
}
