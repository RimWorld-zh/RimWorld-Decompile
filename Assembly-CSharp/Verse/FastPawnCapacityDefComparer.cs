using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B58 RID: 2904
	internal class FastPawnCapacityDefComparer : IEqualityComparer<PawnCapacityDef>
	{
		// Token: 0x06003F74 RID: 16244 RVA: 0x00216B24 File Offset: 0x00214F24
		public bool Equals(PawnCapacityDef x, PawnCapacityDef y)
		{
			return x == y;
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x00216B40 File Offset: 0x00214F40
		public int GetHashCode(PawnCapacityDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04002A1E RID: 10782
		public static readonly FastPawnCapacityDefComparer Instance = new FastPawnCapacityDefComparer();
	}
}
