using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B58 RID: 2904
	internal class FastPawnCapacityDefComparer : IEqualityComparer<PawnCapacityDef>
	{
		// Token: 0x06003F72 RID: 16242 RVA: 0x00216A50 File Offset: 0x00214E50
		public bool Equals(PawnCapacityDef x, PawnCapacityDef y)
		{
			return x == y;
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x00216A6C File Offset: 0x00214E6C
		public int GetHashCode(PawnCapacityDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04002A1E RID: 10782
		public static readonly FastPawnCapacityDefComparer Instance = new FastPawnCapacityDefComparer();
	}
}
