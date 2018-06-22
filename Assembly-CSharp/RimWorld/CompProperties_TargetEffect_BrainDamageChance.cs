using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000750 RID: 1872
	public class CompProperties_TargetEffect_BrainDamageChance : CompProperties
	{
		// Token: 0x06002983 RID: 10627 RVA: 0x00161414 File Offset: 0x0015F814
		public CompProperties_TargetEffect_BrainDamageChance()
		{
			this.compClass = typeof(CompTargetEffect_BrainDamageChance);
		}

		// Token: 0x04001697 RID: 5783
		public float brainDamageChance = 0.3f;
	}
}
