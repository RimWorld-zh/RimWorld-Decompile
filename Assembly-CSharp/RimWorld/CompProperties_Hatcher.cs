using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000717 RID: 1815
	public class CompProperties_Hatcher : CompProperties
	{
		// Token: 0x040015E4 RID: 5604
		public float hatcherDaystoHatch = 1f;

		// Token: 0x040015E5 RID: 5605
		public PawnKindDef hatcherPawn = null;

		// Token: 0x060027E2 RID: 10210 RVA: 0x0015572A File Offset: 0x00153B2A
		public CompProperties_Hatcher()
		{
			this.compClass = typeof(CompHatcher);
		}
	}
}
