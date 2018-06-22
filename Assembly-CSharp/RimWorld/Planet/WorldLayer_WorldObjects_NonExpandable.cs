using System;

namespace RimWorld.Planet
{
	// Token: 0x0200059C RID: 1436
	public class WorldLayer_WorldObjects_NonExpandable : WorldLayer_WorldObjects
	{
		// Token: 0x06001B6F RID: 7023 RVA: 0x000ECD64 File Offset: 0x000EB164
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return worldObject.def.expandingIcon;
		}
	}
}
