using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D9 RID: 729
	public class PawnCapacityFactor
	{
		// Token: 0x04000762 RID: 1890
		public PawnCapacityDef capacity;

		// Token: 0x04000763 RID: 1891
		public float weight = 1f;

		// Token: 0x04000764 RID: 1892
		public float max = 9999f;

		// Token: 0x04000765 RID: 1893
		public bool useReciprocal = false;

		// Token: 0x04000766 RID: 1894
		public float allowedDefect;

		// Token: 0x04000767 RID: 1895
		private const float MaxReciprocalFactor = 5f;

		// Token: 0x06000C0B RID: 3083 RVA: 0x0006AFCC File Offset: 0x000693CC
		public float GetFactor(float capacityEfficiency)
		{
			float num = capacityEfficiency;
			if (this.allowedDefect != 0f && num < 1f)
			{
				num = Mathf.InverseLerp(0f, 1f - this.allowedDefect, num);
			}
			if (num > this.max)
			{
				num = this.max;
			}
			if (this.useReciprocal)
			{
				if (Mathf.Abs(num) < 0.001f)
				{
					num = 5f;
				}
				else
				{
					num = Mathf.Min(1f / num, 5f);
				}
			}
			return num;
		}
	}
}
