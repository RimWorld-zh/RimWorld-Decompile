using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000243 RID: 579
	public class CompProperties_EggLayer : CompProperties
	{
		// Token: 0x06000A72 RID: 2674 RVA: 0x0005EDEC File Offset: 0x0005D1EC
		public CompProperties_EggLayer()
		{
			this.compClass = typeof(CompEggLayer);
		}

		// Token: 0x04000468 RID: 1128
		public float eggLayIntervalDays = 1f;

		// Token: 0x04000469 RID: 1129
		public IntRange eggCountRange = IntRange.one;

		// Token: 0x0400046A RID: 1130
		public ThingDef eggUnfertilizedDef;

		// Token: 0x0400046B RID: 1131
		public ThingDef eggFertilizedDef;

		// Token: 0x0400046C RID: 1132
		public int eggFertilizationCountMax = 1;

		// Token: 0x0400046D RID: 1133
		public bool eggLayFemaleOnly = true;

		// Token: 0x0400046E RID: 1134
		public float eggProgressUnfertilizedMax = 1f;
	}
}
