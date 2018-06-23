using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000715 RID: 1813
	public class CompProperties_Hatcher : CompProperties
	{
		// Token: 0x040015E0 RID: 5600
		public float hatcherDaystoHatch = 1f;

		// Token: 0x040015E1 RID: 5601
		public PawnKindDef hatcherPawn = null;

		// Token: 0x060027DF RID: 10207 RVA: 0x0015537A File Offset: 0x0015377A
		public CompProperties_Hatcher()
		{
			this.compClass = typeof(CompHatcher);
		}
	}
}
