using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000719 RID: 1817
	public class CompProperties_Hatcher : CompProperties
	{
		// Token: 0x060027E5 RID: 10213 RVA: 0x00155149 File Offset: 0x00153549
		public CompProperties_Hatcher()
		{
			this.compClass = typeof(CompHatcher);
		}

		// Token: 0x040015E2 RID: 5602
		public float hatcherDaystoHatch = 1f;

		// Token: 0x040015E3 RID: 5603
		public PawnKindDef hatcherPawn = null;
	}
}
