using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000725 RID: 1829
	public class CompProperties_PlantHarmRadius : CompProperties
	{
		// Token: 0x04001608 RID: 5640
		public SimpleCurve radiusPerDayCurve;

		// Token: 0x04001609 RID: 5641
		public float harmFrequencyPerArea = 1f;

		// Token: 0x06002854 RID: 10324 RVA: 0x00158984 File Offset: 0x00156D84
		public CompProperties_PlantHarmRadius()
		{
			this.compClass = typeof(CompPlantHarmRadius);
		}
	}
}
