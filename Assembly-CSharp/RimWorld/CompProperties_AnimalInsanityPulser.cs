using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200023D RID: 573
	public class CompProperties_AnimalInsanityPulser : CompProperties
	{
		// Token: 0x06000A68 RID: 2664 RVA: 0x0005E6BD File Offset: 0x0005CABD
		public CompProperties_AnimalInsanityPulser()
		{
			this.compClass = typeof(CompAnimalInsanityPulser);
		}

		// Token: 0x04000457 RID: 1111
		public IntRange pulseInterval = new IntRange(60000, 150000);

		// Token: 0x04000458 RID: 1112
		public int radius = 25;
	}
}
