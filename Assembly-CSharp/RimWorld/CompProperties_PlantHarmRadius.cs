using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000723 RID: 1827
	public class CompProperties_PlantHarmRadius : CompProperties
	{
		// Token: 0x04001608 RID: 5640
		public SimpleCurve radiusPerDayCurve;

		// Token: 0x04001609 RID: 5641
		public float harmFrequencyPerArea = 1f;

		// Token: 0x06002850 RID: 10320 RVA: 0x00158834 File Offset: 0x00156C34
		public CompProperties_PlantHarmRadius()
		{
			this.compClass = typeof(CompPlantHarmRadius);
		}
	}
}
