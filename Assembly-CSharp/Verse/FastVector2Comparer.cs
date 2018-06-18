using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D7A RID: 3450
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		// Token: 0x06004D38 RID: 19768 RVA: 0x0028312C File Offset: 0x0028152C
		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x00283148 File Offset: 0x00281548
		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x0400337B RID: 13179
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();
	}
}
