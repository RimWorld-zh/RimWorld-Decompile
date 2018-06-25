using System;

namespace RimWorld.Planet
{
	// Token: 0x0200059E RID: 1438
	public class WorldLayer_WorldObjects_NonExpandable : WorldLayer_WorldObjects
	{
		// Token: 0x06001B72 RID: 7026 RVA: 0x000ED11C File Offset: 0x000EB51C
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return worldObject.def.expandingIcon;
		}
	}
}
