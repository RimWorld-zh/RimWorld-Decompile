using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002DA RID: 730
	public class PawnCapacityOffset
	{
		// Token: 0x04000768 RID: 1896
		public PawnCapacityDef capacity;

		// Token: 0x04000769 RID: 1897
		public float scale = 1f;

		// Token: 0x0400076A RID: 1898
		public float max = 9999f;

		// Token: 0x06000C0D RID: 3085 RVA: 0x0006B084 File Offset: 0x00069484
		public float GetOffset(float capacityEfficiency)
		{
			return (Mathf.Min(capacityEfficiency, this.max) - 1f) * this.scale;
		}
	}
}
