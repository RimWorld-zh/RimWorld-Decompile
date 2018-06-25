using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000754 RID: 1876
	public class CompProperties_TargetEffect_GoodwillImpact : CompProperties
	{
		// Token: 0x0400169C RID: 5788
		public int goodwillImpact = -200;

		// Token: 0x0600298A RID: 10634 RVA: 0x001618A1 File Offset: 0x0015FCA1
		public CompProperties_TargetEffect_GoodwillImpact()
		{
			this.compClass = typeof(CompTargetEffect_GoodwillImpact);
		}
	}
}
