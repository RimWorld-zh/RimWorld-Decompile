using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D77 RID: 3447
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		// Token: 0x06004D4D RID: 19789 RVA: 0x002846DC File Offset: 0x00282ADC
		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x002846F8 File Offset: 0x00282AF8
		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04003386 RID: 13190
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();
	}
}
