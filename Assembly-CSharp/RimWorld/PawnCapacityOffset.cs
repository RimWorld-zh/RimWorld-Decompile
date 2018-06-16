using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D8 RID: 728
	public class PawnCapacityOffset
	{
		// Token: 0x06000C0C RID: 3084 RVA: 0x0006AEC4 File Offset: 0x000692C4
		public float GetOffset(float capacityEfficiency)
		{
			return (Mathf.Min(capacityEfficiency, this.max) - 1f) * this.scale;
		}

		// Token: 0x04000766 RID: 1894
		public PawnCapacityDef capacity;

		// Token: 0x04000767 RID: 1895
		public float scale = 1f;

		// Token: 0x04000768 RID: 1896
		public float max = 9999f;
	}
}
