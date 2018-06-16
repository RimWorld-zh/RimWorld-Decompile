using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000727 RID: 1831
	public class CompProperties_PlantHarmRadius : CompProperties
	{
		// Token: 0x06002856 RID: 10326 RVA: 0x00158600 File Offset: 0x00156A00
		public CompProperties_PlantHarmRadius()
		{
			this.compClass = typeof(CompPlantHarmRadius);
		}

		// Token: 0x0400160A RID: 5642
		public SimpleCurve radiusPerDayCurve;

		// Token: 0x0400160B RID: 5643
		public float harmFrequencyPerArea = 1f;
	}
}
