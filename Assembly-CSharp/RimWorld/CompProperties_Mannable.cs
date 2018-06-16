using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024A RID: 586
	public class CompProperties_Mannable : CompProperties
	{
		// Token: 0x06000A80 RID: 2688 RVA: 0x0005F30B File Offset: 0x0005D70B
		public CompProperties_Mannable()
		{
			this.compClass = typeof(CompMannable);
		}

		// Token: 0x04000496 RID: 1174
		public WorkTags manWorkType = WorkTags.None;
	}
}
