using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B54 RID: 2900
	internal class FastPawnCapacityDefComparer : IEqualityComparer<PawnCapacityDef>
	{
		// Token: 0x04002A1C RID: 10780
		public static readonly FastPawnCapacityDefComparer Instance = new FastPawnCapacityDefComparer();

		// Token: 0x06003F74 RID: 16244 RVA: 0x00217150 File Offset: 0x00215550
		public bool Equals(PawnCapacityDef x, PawnCapacityDef y)
		{
			return x == y;
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x0021716C File Offset: 0x0021556C
		public int GetHashCode(PawnCapacityDef obj)
		{
			return obj.GetHashCode();
		}
	}
}
