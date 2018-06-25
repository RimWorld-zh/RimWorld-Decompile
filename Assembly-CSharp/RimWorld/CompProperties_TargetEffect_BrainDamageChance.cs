using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000752 RID: 1874
	public class CompProperties_TargetEffect_BrainDamageChance : CompProperties
	{
		// Token: 0x0400169B RID: 5787
		public float brainDamageChance = 0.3f;

		// Token: 0x06002986 RID: 10630 RVA: 0x001617C4 File Offset: 0x0015FBC4
		public CompProperties_TargetEffect_BrainDamageChance()
		{
			this.compClass = typeof(CompTargetEffect_BrainDamageChance);
		}
	}
}
