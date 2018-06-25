using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000754 RID: 1876
	public class CompProperties_TargetEffect_GoodwillImpact : CompProperties
	{
		// Token: 0x04001698 RID: 5784
		public int goodwillImpact = -200;

		// Token: 0x0600298B RID: 10635 RVA: 0x00161641 File Offset: 0x0015FA41
		public CompProperties_TargetEffect_GoodwillImpact()
		{
			this.compClass = typeof(CompTargetEffect_GoodwillImpact);
		}
	}
}
