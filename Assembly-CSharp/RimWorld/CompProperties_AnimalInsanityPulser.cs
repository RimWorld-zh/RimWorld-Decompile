using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200023D RID: 573
	public class CompProperties_AnimalInsanityPulser : CompProperties
	{
		// Token: 0x06000A66 RID: 2662 RVA: 0x0005E719 File Offset: 0x0005CB19
		public CompProperties_AnimalInsanityPulser()
		{
			this.compClass = typeof(CompAnimalInsanityPulser);
		}

		// Token: 0x04000455 RID: 1109
		public IntRange pulseInterval = new IntRange(60000, 150000);

		// Token: 0x04000456 RID: 1110
		public int radius = 25;
	}
}
