using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024C RID: 588
	public class CompProperties_Mannable : CompProperties
	{
		// Token: 0x04000494 RID: 1172
		public WorkTags manWorkType = WorkTags.None;

		// Token: 0x06000A82 RID: 2690 RVA: 0x0005F4B7 File Offset: 0x0005D8B7
		public CompProperties_Mannable()
		{
			this.compClass = typeof(CompMannable);
		}
	}
}
