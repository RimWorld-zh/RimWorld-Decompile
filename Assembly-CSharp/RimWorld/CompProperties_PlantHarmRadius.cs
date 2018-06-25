using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000725 RID: 1829
	public class CompProperties_PlantHarmRadius : CompProperties
	{
		// Token: 0x0400160C RID: 5644
		public SimpleCurve radiusPerDayCurve;

		// Token: 0x0400160D RID: 5645
		public float harmFrequencyPerArea = 1f;

		// Token: 0x06002853 RID: 10323 RVA: 0x00158BE4 File Offset: 0x00156FE4
		public CompProperties_PlantHarmRadius()
		{
			this.compClass = typeof(CompPlantHarmRadius);
		}
	}
}
