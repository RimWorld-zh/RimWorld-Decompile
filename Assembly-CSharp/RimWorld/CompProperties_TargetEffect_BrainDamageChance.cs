using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000754 RID: 1876
	public class CompProperties_TargetEffect_BrainDamageChance : CompProperties
	{
		// Token: 0x06002988 RID: 10632 RVA: 0x001611A8 File Offset: 0x0015F5A8
		public CompProperties_TargetEffect_BrainDamageChance()
		{
			this.compClass = typeof(CompTargetEffect_BrainDamageChance);
		}

		// Token: 0x04001699 RID: 5785
		public float brainDamageChance = 0.3f;
	}
}
