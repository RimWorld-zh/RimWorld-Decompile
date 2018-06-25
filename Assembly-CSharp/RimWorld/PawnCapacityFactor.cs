using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D9 RID: 729
	public class PawnCapacityFactor
	{
		// Token: 0x0400075F RID: 1887
		public PawnCapacityDef capacity;

		// Token: 0x04000760 RID: 1888
		public float weight = 1f;

		// Token: 0x04000761 RID: 1889
		public float max = 9999f;

		// Token: 0x04000762 RID: 1890
		public bool useReciprocal = false;

		// Token: 0x04000763 RID: 1891
		public float allowedDefect;

		// Token: 0x04000764 RID: 1892
		private const float MaxReciprocalFactor = 5f;

		// Token: 0x06000C0C RID: 3084 RVA: 0x0006AFC4 File Offset: 0x000693C4
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
