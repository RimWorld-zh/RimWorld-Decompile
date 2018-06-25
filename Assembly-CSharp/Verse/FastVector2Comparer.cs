using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D79 RID: 3449
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		// Token: 0x04003386 RID: 13190
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();

		// Token: 0x06004D51 RID: 19793 RVA: 0x00284808 File Offset: 0x00282C08
		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x00284824 File Offset: 0x00282C24
		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}
	}
}
