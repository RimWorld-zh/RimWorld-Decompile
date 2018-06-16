using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000243 RID: 579
	public class CompProperties_EggLayer : CompProperties
	{
		// Token: 0x06000A74 RID: 2676 RVA: 0x0005ED90 File Offset: 0x0005D190
		public CompProperties_EggLayer()
		{
			this.compClass = typeof(CompEggLayer);
		}

		// Token: 0x0400046A RID: 1130
		public float eggLayIntervalDays = 1f;

		// Token: 0x0400046B RID: 1131
		public IntRange eggCountRange = IntRange.one;

		// Token: 0x0400046C RID: 1132
		public ThingDef eggUnfertilizedDef;

		// Token: 0x0400046D RID: 1133
		public ThingDef eggFertilizedDef;

		// Token: 0x0400046E RID: 1134
		public int eggFertilizationCountMax = 1;

		// Token: 0x0400046F RID: 1135
		public bool eggLayFemaleOnly = true;

		// Token: 0x04000470 RID: 1136
		public float eggProgressUnfertilizedMax = 1f;
	}
}
