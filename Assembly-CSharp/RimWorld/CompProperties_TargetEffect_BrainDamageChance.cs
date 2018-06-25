using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000752 RID: 1874
	public class CompProperties_TargetEffect_BrainDamageChance : CompProperties
	{
		// Token: 0x04001697 RID: 5783
		public float brainDamageChance = 0.3f;

		// Token: 0x06002987 RID: 10631 RVA: 0x00161564 File Offset: 0x0015F964
		public CompProperties_TargetEffect_BrainDamageChance()
		{
			this.compClass = typeof(CompTargetEffect_BrainDamageChance);
		}
	}
}
