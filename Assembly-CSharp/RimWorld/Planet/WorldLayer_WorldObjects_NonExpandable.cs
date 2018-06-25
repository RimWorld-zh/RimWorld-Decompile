using System;

namespace RimWorld.Planet
{
	// Token: 0x0200059E RID: 1438
	public class WorldLayer_WorldObjects_NonExpandable : WorldLayer_WorldObjects
	{
		// Token: 0x06001B73 RID: 7027 RVA: 0x000ECEB4 File Offset: 0x000EB2B4
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return worldObject.def.expandingIcon;
		}
	}
}
