using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B57 RID: 2903
	internal class FastPawnCapacityDefComparer : IEqualityComparer<PawnCapacityDef>
	{
		// Token: 0x04002A23 RID: 10787
		public static readonly FastPawnCapacityDefComparer Instance = new FastPawnCapacityDefComparer();

		// Token: 0x06003F77 RID: 16247 RVA: 0x0021750C File Offset: 0x0021590C
		public bool Equals(PawnCapacityDef x, PawnCapacityDef y)
		{
			return x == y;
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x00217528 File Offset: 0x00215928
		public int GetHashCode(PawnCapacityDef obj)
		{
			return obj.GetHashCode();
		}
	}
}
