using System;

namespace RimWorld.Planet
{
	// Token: 0x020005A0 RID: 1440
	public class WorldLayer_WorldObjects_NonExpandable : WorldLayer_WorldObjects
	{
		// Token: 0x06001B78 RID: 7032 RVA: 0x000ECD10 File Offset: 0x000EB110
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return worldObject.def.expandingIcon;
		}
	}
}
