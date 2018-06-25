using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B56 RID: 2902
	internal class FastPawnCapacityDefComparer : IEqualityComparer<PawnCapacityDef>
	{
		// Token: 0x04002A1C RID: 10780
		public static readonly FastPawnCapacityDefComparer Instance = new FastPawnCapacityDefComparer();

		// Token: 0x06003F77 RID: 16247 RVA: 0x0021722C File Offset: 0x0021562C
		public bool Equals(PawnCapacityDef x, PawnCapacityDef y)
		{
			return x == y;
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x00217248 File Offset: 0x00215648
		public int GetHashCode(PawnCapacityDef obj)
		{
			return obj.GetHashCode();
		}
	}
}
