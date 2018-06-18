using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C2C RID: 3116
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		// Token: 0x06004475 RID: 17525 RVA: 0x0023F4FC File Offset: 0x0023D8FC
		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x0023F518 File Offset: 0x0023D918
		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}

		// Token: 0x04002E72 RID: 11890
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();
	}
}
