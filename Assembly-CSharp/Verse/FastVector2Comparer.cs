using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D7A RID: 3450
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		// Token: 0x0400338D RID: 13197
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();

		// Token: 0x06004D51 RID: 19793 RVA: 0x00284AE8 File Offset: 0x00282EE8
		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x00284B04 File Offset: 0x00282F04
		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}
	}
}
