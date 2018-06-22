using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000752 RID: 1874
	public class CompProperties_TargetEffect_GoodwillImpact : CompProperties
	{
		// Token: 0x06002987 RID: 10631 RVA: 0x001614F1 File Offset: 0x0015F8F1
		public CompProperties_TargetEffect_GoodwillImpact()
		{
			this.compClass = typeof(CompTargetEffect_GoodwillImpact);
		}

		// Token: 0x04001698 RID: 5784
		public int goodwillImpact = -200;
	}
}
