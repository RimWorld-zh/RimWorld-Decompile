using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024A RID: 586
	public class CompProperties_Mannable : CompProperties
	{
		// Token: 0x04000494 RID: 1172
		public WorkTags manWorkType = WorkTags.None;

		// Token: 0x06000A7E RID: 2686 RVA: 0x0005F367 File Offset: 0x0005D767
		public CompProperties_Mannable()
		{
			this.compClass = typeof(CompMannable);
		}
	}
}
