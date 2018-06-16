using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D7B RID: 3451
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		// Token: 0x06004D3A RID: 19770 RVA: 0x0028314C File Offset: 0x0028154C
		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		// Token: 0x06004D3B RID: 19771 RVA: 0x00283168 File Offset: 0x00281568
		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x0400337D RID: 13181
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();
	}
}
