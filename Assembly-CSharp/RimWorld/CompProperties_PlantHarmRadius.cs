using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000727 RID: 1831
	public class CompProperties_PlantHarmRadius : CompProperties
	{
		// Token: 0x06002858 RID: 10328 RVA: 0x00158678 File Offset: 0x00156A78
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
