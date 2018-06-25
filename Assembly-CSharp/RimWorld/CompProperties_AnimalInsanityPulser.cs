using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200023F RID: 575
	public class CompProperties_AnimalInsanityPulser : CompProperties
	{
		// Token: 0x04000457 RID: 1111
		public IntRange pulseInterval = new IntRange(60000, 150000);

		// Token: 0x04000458 RID: 1112
		public int radius = 25;

		// Token: 0x06000A69 RID: 2665 RVA: 0x0005E865 File Offset: 0x0005CC65
		public CompProperties_AnimalInsanityPulser()
		{
			this.compClass = typeof(CompAnimalInsanityPulser);
		}
	}
}
