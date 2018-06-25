using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024C RID: 588
	public class CompProperties_Mannable : CompProperties
	{
		// Token: 0x04000496 RID: 1174
		public WorkTags manWorkType = WorkTags.None;

		// Token: 0x06000A81 RID: 2689 RVA: 0x0005F4B3 File Offset: 0x0005D8B3
		public CompProperties_Mannable()
		{
			this.compClass = typeof(CompMannable);
		}
	}
}
