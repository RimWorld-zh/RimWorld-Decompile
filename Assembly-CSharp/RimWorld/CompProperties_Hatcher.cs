using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000717 RID: 1815
	public class CompProperties_Hatcher : CompProperties
	{
		// Token: 0x040015E0 RID: 5600
		public float hatcherDaystoHatch = 1f;

		// Token: 0x040015E1 RID: 5601
		public PawnKindDef hatcherPawn = null;

		// Token: 0x060027E3 RID: 10211 RVA: 0x001554CA File Offset: 0x001538CA
		public CompProperties_Hatcher()
		{
			this.compClass = typeof(CompHatcher);
		}
	}
}
