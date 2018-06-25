using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200023F RID: 575
	public class CompProperties_AnimalInsanityPulser : CompProperties
	{
		// Token: 0x04000455 RID: 1109
		public IntRange pulseInterval = new IntRange(60000, 150000);

		// Token: 0x04000456 RID: 1110
		public int radius = 25;

		// Token: 0x06000A6A RID: 2666 RVA: 0x0005E869 File Offset: 0x0005CC69
		public CompProperties_AnimalInsanityPulser()
		{
			this.compClass = typeof(CompAnimalInsanityPulser);
		}
	}
}
