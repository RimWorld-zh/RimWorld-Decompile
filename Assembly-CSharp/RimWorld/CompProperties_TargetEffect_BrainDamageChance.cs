using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000754 RID: 1876
	public class CompProperties_TargetEffect_BrainDamageChance : CompProperties
	{
		// Token: 0x0600298A RID: 10634 RVA: 0x0016123C File Offset: 0x0015F63C
		public CompProperties_TargetEffect_BrainDamageChance()
		{
			this.compClass = typeof(CompTargetEffect_BrainDamageChance);
		}

		// Token: 0x04001699 RID: 5785
		public float brainDamageChance = 0.3f;
	}
}
